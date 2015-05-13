using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using IDCM.Base;
using IDCM.Core;
using NPOI.SS.Util;
using IDCM.ComPO;

namespace IDCM.DataTransfer
{
    class GCMExcelExporter
    {
        public bool exportExcel(GCMTableCache gtcache, string fpath, bool exportDetail=false)
        {
            try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(fpath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Core Datasets");
                HashSet<int> excludes = new HashSet<int>();
                int tcount = gtcache.getOverViewRowCount();
                Dictionary<string, int> maps = gtcache.getOverViewIAttrMapping();
                IRow columnHead = sheet.CreateRow(0);
                //填充列头
                int i = 0;
                foreach (string key in maps.Keys)
                {
                    ICell cell = columnHead.CreateCell(i++, CellType.String);
                    cell.SetCellValue(key);
                    cell.CellStyle.FillBackgroundColor = IndexedColors.Green.Index;
                }
                CellRangeAddress cra = CellRangeAddress.ValueOf("A1:" + numToExcelIndex(maps.Count) + "1");
                sheet.SetAutoFilter(cra);
                //填充dgv单元格内容
                int ridx = 1;
                while (ridx < tcount)
                {
                    Dictionary<int, string> drow = gtcache.getOverViewIRow(ridx);
                    if (drow != null)
                    {
                        IRow srow = sheet.CreateRow(i + 1);
                        mergeDataToSheetRow(maps, drow, srow);
                        if (exportDetail)
                        {
                            throw new NotImplementedException();
                        }
                    }
                    ridx++;
                }
                using (FileStream fs = File.Create(fpath))
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

        public bool exportExcel(GCMTableCache gtcache, string fpath, int[] selectedRows, bool exportDetail=false)
        {
            try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(fpath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Core Datasets");
                HashSet<int> excludes = new HashSet<int>();

                IRow columnHead = sheet.CreateRow(0);
                Dictionary<string, int> maps = gtcache.getOverViewIAttrMapping();
                //填充dgv单元格内容
                int IrowIndex = 1;
                for (int ridx = selectedRows.Length - 1; ridx >= 0; ridx--)
                {
                    int lindex = selectedRows[ridx];
                    Dictionary<int, string> drow = gtcache.getOverViewIRow(lindex);
                    if(drow!=null)
                    {
                        IRow srow = sheet.CreateRow(IrowIndex);
                        mergeDataToSheetRow(maps, drow, srow);
                        if (exportDetail)
                        {
                            throw new NotImplementedException();
                        }
                    }
                    IrowIndex++;
                }
                using (FileStream fs = File.Create(fpath))
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
        protected void mergeDataToSheetRow(Dictionary<string, int> customAttrDBMapping, Dictionary<int, string> row, IRow srow)
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
