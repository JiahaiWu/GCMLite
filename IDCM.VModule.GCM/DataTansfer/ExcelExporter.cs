using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Core;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System.IO;
using IDCM.Base;
using System.Windows.Forms;
using System.Data;

namespace IDCM.DataTansfer
{
    class ExcelExporter
    {
        /// <summary>
        /// 导出数据到excel，数据源从数据库读取
        /// </summary>
        /// <param name="datasource">DataSourceMHub句柄，主要封装WorkSpaceManager</param>
        /// <param name="filepath">导出路径</param>
        /// <param name="cmdstr">查询字符串</param>
        /// <param name="tcount">总记录数</param>
        /// <returns></returns>
        public bool exportExcel(CTableCache ctableCache, string filepath)
        {
            try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(filepath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Core Datasets");
                IRow rowHead = sheet.CreateRow(0);
                HashSet<int> excludes = new HashSet<int>();
                int tcount = ctableCache.getRowCount();
                //填写表头
                Dictionary<string, int> maps = ctableCache.getIAttrMapping();
                //填写表头
                int i = 0;
                foreach (string key in maps.Keys)
                {
                    ICell cell = rowHead.CreateCell(i++, CellType.String);
                    cell.SetCellValue(key);
                    cell.CellStyle.FillBackgroundColor = IndexedColors.Green.Index;
                }
                CellRangeAddress cra = CellRangeAddress.ValueOf("A1:" + numToExcelIndex(maps.Count) + "1");
                sheet.SetAutoFilter(cra);
                //填写内容
                int ridx = 1;
                while (ridx < tcount)
                {
                    Dictionary<int, string> drow = ctableCache.getIRow(ridx);
                    if(drow!=null)
                    {
                        IRow srow = sheet.CreateRow(ridx++);
                        mergeDataToSheetRow(maps, drow, srow);
                    }
                }
                using (FileStream fs = File.Create(filepath))
                {
                    workbook.Write(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in exportExcel(...)", ex);
            }
            return true;
        }

        /// <summary>
        /// 导出数据到excel，数据源DataGridViewSelectedRowCollection
        /// </summary>
        /// <param name="datasource">DataSourceMHub句柄，主要封装WorkSpaceManager</param>
        /// <param name="filepath">导出路径</param>
        /// <param name="selectedRows">数据源</param>
        /// <returns></returns>
        public bool exportExcel(CTableCache ctableCache, string filepath, int[] selectedRows)
        {
            try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(filepath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Core Datasets");
                IRow rowHead = sheet.CreateRow(0);
                HashSet<int> excludes = new HashSet<int>();
                //填写表头
                Dictionary<string, int> maps = ctableCache.getIAttrMapping();
                //填写表头
                int i = 0;
                foreach (string key in maps.Keys)
                {
                    ICell cell = rowHead.CreateCell(i++, CellType.String);
                    cell.SetCellValue(key);
                    cell.CellStyle.FillBackgroundColor = IndexedColors.Green.Index;
                }
                CellRangeAddress cra = CellRangeAddress.ValueOf("A1:" + numToExcelIndex(maps.Count) + "1");
                sheet.SetAutoFilter(cra);
                //填写内容
                int IrowIndex = 1;
                for (int ridx = selectedRows.Length - 1; ridx >= 0; ridx--)
                {
                    int lindex = selectedRows[ridx];
                    Dictionary<int, string> drow = ctableCache.getIRow(lindex);
                    if(drow!=null)
                    {
                        IRow srow = sheet.CreateRow(IrowIndex++);
                        mergeDataToSheetRow(maps, drow, srow);
                    }
                }
                using (FileStream fs = File.Create(filepath))
                {
                    workbook.Write(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in exportExcel(...)", ex);
            }
            return true;
        }
        /// <summary>
        /// 根据字段将一行记录转换成IRow
        /// </summary>
        /// <param name="customAttrDBMapping">字段名称</param>
        /// <param name="row">一条记录</param>
        /// <param name="srow">IRow</param>
        protected void mergeDataToSheetRow(Dictionary<string, int> customAttrDBMapping, Dictionary<int,string> row, IRow srow)
        {
            int idx = 0;
            foreach (KeyValuePair<string, int> kvpair in customAttrDBMapping)
            {
                if (kvpair.Value > 0)
                {
                    int k = kvpair.Value;
                    string value = row[k].ToString();
                    srow.CreateCell(idx).SetCellValue(value);
                }
                else
                {
                    srow.CreateCell(idx);
                }
                ++idx;
            }
        }
        /// <summary>
        /// 用于针对Excel的列名转换实现，1->A
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string numToExcelIndex(int value)
        {
            if (value < 1 || value > 18277)
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(value > 0 && value < 18278);
#endif
                return null;
            }
            string rtn = string.Empty;
            List<int> iList = new List<int>();
            //To single Int
            while (value / 26 != 0 || value % 26 != 0)
            {
                iList.Add(value % 26);
                value /= 26;
            }
            //Change 0 To 26
            for (int j = 0; j < iList.Count - 1; j++)
            {
                if (iList[j] == 0)
                {
                    iList[j + 1] -= 1;
                    iList[j] = 26;
                }
            }
            //Remove 0 at last
            if (iList[iList.Count - 1] == 0)
            {
                iList.Remove(iList[iList.Count - 1]);
            }
            //To String
            for (int j = iList.Count - 1; j >= 0; j--)
            {
                char c = (char)(iList[j] + 64);
                rtn += c.ToString();
            }
            return rtn;
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
