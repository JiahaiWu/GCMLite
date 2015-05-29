using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using System.Windows.Forms;
using System.Xml;
using IDCM.Forms;
using System.IO;
using System.Configuration;
using IDCM.Base;
using Newtonsoft.Json;
using IDCM.Base.Utils;
using IDCM.MsgDriver;

namespace IDCM.Core
{
    class DataImportChecker
    {
        #region Methods

        /// <summary>
        /// 解析指定的Excel文档，验证数据转换的属性映射条件.
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        internal  static bool checkForExcelImport(string fpath, ref Dictionary<string, string> dataMapping)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(fpath);
            IWorkbook workbook = null;
            try
            {
                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    workbook = WorkbookFactory.Create(fs);
                    ISheet dataSheet = workbook.GetSheet("Core Datasets");
                    if (dataSheet == null)
                        dataSheet = workbook.GetSheetAt(0);
                    return fetchSheetMappingInfo(dataSheet, ref dataMapping) && dataMapping.Count > 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to import excel file")+"！ ", ex);
#if DEBUG
                DCMPublisher.noteSimpleMsg("Excel: " + IDCM.Base.GlobalTextRes.Text("Failed to import excel file") + "！ " + ex.Message + "\n" + ex.ToString(),DCMMsgType.Alert);
#else
                DCMPublisher.noteSimpleMsg("Excel: " + IDCM.Base.GlobalTextRes.Text("Failed to import excel file") + "！ " + ex.Message,DCMMsgType.Alert);
#endif
            }
            return false;
        }
        /// <summary>
        /// 解析指定的XML文档，验证数据转换的属性映射条件.
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        internal static bool checkForXMLImport(string fpath, ref Dictionary<string, string> dataMapping)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(fpath);
            try
            {
                XmlDocument xDoc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                using (XmlReader xRead = XmlReader.Create(fullPath))
                {
                    xDoc.Load(xRead);
                    return fetchXMLMappingInfo(xDoc, ref dataMapping) && dataMapping.Count > 0;
                }
            }
            catch (Exception ex)
            {
                log.Error("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import excel file") + "！ ", ex);
#if DEBUG
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import excel file") + "！ " + ex.Message + "\n" + ex.ToString(), DCMMsgType.Alert);
#else
                                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import excel file") + "！ " + ex.Message, DCMMsgType.Alert);
#endif
            }
            return false;
        }
        internal static bool checkForMDIImport(string fpath)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(fpath);
            try
            {
                ///////////////////////////////////////////////////////////////
                //String jsonStr = FileUtil.readAsUTF8Text(fullPath);
                //var obj = JsonConvert.DeserializeObjectAsync(jsonStr);
                //暂时不用
                ///////////////////////////////////////////////////////////////
                return true;
            }
            catch (Exception ex)
            {
                log.Error("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import dump file") + "！ ", ex);
#if DEBUG
                DCMPublisher.noteSimpleMsg("ERROR:" + IDCM.Base.GlobalTextRes.Text("Failed to import dump file") + "！ " + ex.Message + "\n" + ex.ToString(), DCMMsgType.Alert);
#else
                DCMPublisher.noteSimpleMsg("ERROR:" + IDCM.Base.GlobalTextRes.Text("Failed to import dump file") + "！ " + ex.Message, DCMMsgType.Alert);
#endif
            }
            return false;
        }

        private static bool fetchXMLMappingInfo(XmlDocument xDoc, ref Dictionary<string, string> dataMapping)
        {
            XmlNodeList strainChildNodes = xDoc.DocumentElement.ChildNodes;
            //一直向下探索，直到某个节点下没有子节点，说明这个节点是个attrNode,

            //节点探测代码
            XmlNode strainNode = strainChildNodes[0];//获取第一个strainNode
            List<string> attrNameList = new List<string>(strainChildNodes.Count);
            int cursor = Convert.ToInt32(ConfigurationManager.AppSettings.Get(SysConstants.Cursor));
            int detectDepth = Convert.ToInt32(ConfigurationManager.AppSettings.Get(SysConstants.DetectDepth));
            double GrowthFactor = Convert.ToDouble(ConfigurationManager.AppSettings.Get(SysConstants.GrowthFactor));
            while (!(strainNode == null))
            {
                if (cursor > detectDepth)
                    break;
                if (mergeAttrList(attrNameList, strainNode.ChildNodes))//如果这个节点下有新属性出现，使探测深度增加2倍
                    detectDepth = (int)(detectDepth * GrowthFactor);
                strainNode = strainNode.NextSibling;
                cursor++;
            }
            ///////////////////////////////////////////////////////////////
            using (AttrMapOptionDlg amoDlg = new AttrMapOptionDlg())
            {
                amoDlg.BringToFront();
                amoDlg.setInitCols(attrNameList, Core.CustomColDefGetter.getCustomCols(), ref dataMapping);
                amoDlg.ShowDialog();
                ///////////////////////////////////////////
                if (amoDlg.DialogResult == DialogResult.OK)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 通过NPOI读取Excel文档，转换可识别内容至本地数据库中
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dgv"></param>
        private static bool fetchSheetMappingInfo(ISheet sheet, ref Dictionary<string, string> dataMapping)
        {
            int skipIdx = 1;
            if (sheet == null || sheet.LastRowNum < skipIdx) //no data
                return false;
            /////////////////////////////////////////////////////////
            IRow titleRow = sheet.GetRow(skipIdx - 1);
            int columnSize = titleRow.LastCellNum;
            int rowSize = sheet.LastRowNum;
            List<string> xlscols = new List<string>(columnSize);
            for (int i = titleRow.FirstCellNum; i < columnSize; i++)
            {
                ICell titleCell = titleRow.GetCell(i);
                if (titleCell != null && titleCell.ToString().Length > 0)
                {
                    string cellData = titleCell.ToString();
                    xlscols.Add(cellData.Trim());
                }
                else
                {
                    xlscols.Add(null);
                }
            }
            ///////////////////////////////////////////////////////////////
            using (AttrMapOptionDlg amoDlg = new AttrMapOptionDlg())
            {
                amoDlg.BringToFront();
                amoDlg.setInitCols(xlscols, Core.CustomColDefGetter.getCustomCols(), ref dataMapping);
                amoDlg.ShowDialog();
                ///////////////////////////////////////////
                if (amoDlg.DialogResult == DialogResult.OK)
                    return true;
            }
            return false;
        }

        private static bool mergeAttrList(List<string> attrNameList, XmlNodeList attrNodeList)
        {
            int startLeng = attrNameList.Count;
            foreach (XmlNode strainChildNode in attrNodeList)
            {
                if (!attrNameList.Contains(strainChildNode.Name))
                    attrNameList.Add(strainChildNode.Name);
            }
            int endLeng = attrNameList.Count;
            if (startLeng != endLeng)
                return true;
            return false;
        }
        #endregion

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
