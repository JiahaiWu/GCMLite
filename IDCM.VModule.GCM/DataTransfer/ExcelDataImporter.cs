using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using System.IO;
using System.Windows.Forms;
using IDCM.MsgDriver;
using IDCM.Base.ComPO;
using IDCM.Core;
using System.Collections.Concurrent;
using System.Data;

namespace IDCM.DataTransfer
{
    class ExcelDataImporter
    {
        /// <summary>
        /// 解析指定的Excel文档，执行数据转换.
        /// 本方法调用对类功能予以线程包装，用于异步调用如何方法。
        /// 在本线程调用下的控件调用，需通过UI控件的Invoke/BegainInvoke方法更新。
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns>返回请求流程是否执行完成</returns>
        public static bool parseExcelData(CTableCache ctcache, string fpath, ref Dictionary<string, string> dataMapping)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(fpath);
            IWorkbook workbook = null;
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                workbook = WorkbookFactory.Create(fs);
                ISheet dataSheet = workbook.GetSheet("Core Datasets");
                if (dataSheet == null)
                    dataSheet = workbook.GetSheetAt(0);
                parseSheetInfo(ctcache, dataSheet, ref dataMapping);
            }
            return true;
        }
        /// <summary>
        /// 通过NPOI读取Excel文档，转换可识别内容
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dgv"></param>
        public static void parseSheetInfo(CTableCache ctcache, ISheet sheet, ref Dictionary<string, string> dataMapping)
        {
            int skipIdx = 1;
            if (sheet == null || sheet.LastRowNum < skipIdx) //no data
                return;
            /////////////////////////////////////////////////////////
            IRow titleRow = sheet.GetRow(skipIdx - 1);
            int columnSize = titleRow.LastCellNum;
            int rowSize = sheet.LastRowNum;
            Dictionary<string, short> ExcelMapIdxs = new Dictionary<string, short>();
            for (short i = titleRow.FirstCellNum; i < columnSize; i++)
            {
                ICell titleCell = titleRow.GetCell(i);
                if (titleCell != null && titleCell.ToString().Length > 0)
                {
                    string cellData = titleCell.ToString();
                    if (dataMapping.ContainsKey(cellData))
                    {
                        ExcelMapIdxs[cellData] = i ;
                    }
                }
            }
            Dictionary<short, string> convertMapIdxs = new Dictionary<short,string>();
            foreach(KeyValuePair<string,string> pair in dataMapping)
            {
                short fid = -1;
                if (ExcelMapIdxs.TryGetValue(pair.Key, out fid) && pair.Value!=null && pair.Value.Length>0)
                    convertMapIdxs[fid] = pair.Value;
            }
            ///////////////////////////////////////////////////////////////
            if (dataMapping != null && dataMapping.Count > 0)
            {
                for (int i = skipIdx; i <= rowSize; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　
                    ICell headCell = row.GetCell(row.FirstCellNum);
                    if (headCell == null || headCell.ToString().Length == 0 || headCell.ToString().Equals("end!"))
                        break;
                    lock (ctcache.GSyncRoot)
                    {
                        Dictionary<string, string> mapValues = new Dictionary<string, string>();
                        foreach (KeyValuePair<short, string> kv in convertMapIdxs)
                        {
                            if (kv.Key >= row.FirstCellNum)
                            {
                                ICell cell = row.GetCell(kv.Key);
                                if (cell != null)
                                {
                                    string cellData = cell.ToString().Trim();
                                    mapValues[kv.Value] = cellData;
                                }
                            }
                        }
                        ctcache.addRow(mapValues);
                    }
                }
            }
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
