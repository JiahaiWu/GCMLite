using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.ComPO;
using IDCM.Base;
using IDCM.Base.Utils;
using System.IO;
using System.Configuration;
using IDCM.Base.AbsInterfaces;
using NPOI.SS.UserModel;
using System.Windows.Forms;
using System.Xml;
using IDCM.Forms;
using IDCM.BGHandler;
using IDCM.ComponentUtil;
using DCMControlLib;
using System.Diagnostics;
using IDCM.Core;
using IDCM.BGHandlerManager;
using IDCM.DataTransfer;

namespace IDCM.ViewManager
{
    class LocalServManager
    {
        public LocalServManager(CTableCache ctcache)
        {
            this.ctcache = ctcache;
        }
        /// <summary>
        /// 导入数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void importData(string fpath)
        {
            Dictionary<string, string> dataMapping = new Dictionary<string, string>();
            AbsBGHandler eih = null;
            if (fpath.ToLower().EndsWith(".xls") || fpath.ToLower().EndsWith(".xlsx"))
            {
                if (DataImportChecker.checkForExcelImport(fpath, ref dataMapping))
                {
                    eih = new ExcelImportHandler(ctcache, fpath, ref dataMapping);
                }
            }
            if (fpath.ToLower().EndsWith(".xml"))
            {
                if (DataImportChecker.checkForXMLImport(fpath, ref dataMapping))
                {
                    eih = new XMLImportHandler(ctcache, fpath, ref dataMapping);
                }
            }
            if (fpath.ToLower().EndsWith(".mdi"))
            {
                if (DataImportChecker.checkForMDIImport(fpath))
                {
                    eih = new MDIImportHandler(ctcache, fpath);
                }
            }
            if(eih!=null)
                BGWorkerInvoker.pushHandler(eih);
        }
        /// <summary>
        /// 导出数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void exportData(DataGridView dgv)
        {
            DataGridViewSelectedRowCollection selectedRows = dgv.SelectedRows;
            ExportTypeDlg exportDlg = new ExportTypeDlg();
            if (exportDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    AbsBGHandler handler = null;
                    ExportType etype = ExportTypeDlg.LastOptionValue;
                    string epath = ExportTypeDlg.LastFilePath;
                    switch (etype)
                    {
                        case ExportType.Excel:
                            handler = new ExcelExportHandler(ctcache, epath);
                            break;
                        //case ExportType.JSONList:
                        //    handler = new JSONListExportHandler(ctableCache.DTable, epath, selectedRows);
                        //    break;
                        //case ExportType.TSV:
                        //    handler = new TextExportHandler(ctableCache.DTable, epath, selectedRows,"\t");
                        //    break;
                        //case ExportType.CSV:
                        //    handler = new TextExportHandler(ctableCache.DTable, epath, selectedRows,",");
                        //    break;
                        //case ExportType.XML:
                        //    handler = new XMLExportHandler(ctableCache.DTable, epath, selectedRows);
                        //    break;
                        default:
                            MessageBox.Show("Unsupport export type!");
                            break;
                    }
                    if(handler!=null)
                        BGWorkerInvoker.pushHandler(handler);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据导出失败。");
                    log.Info("数据导出失败，错误信息：", ex);
                }
            }
        }

        /// <summary>
        /// 校验数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void checkData(DataGridView dgv)
        {
        }
        public void checkData(DataGridViewSelectedRowCollection selectedRows)
        {
        }
        public void checkData(params DataGridViewRow[] selectedRows)
        {
        }


        internal void publishLocalRowsToGCM(params DataGridViewRow[] selectedRows)
        {
            
        }

        internal void publishLocalRowsToGCM(DataGridViewSelectedRowCollection selectedRows)
        {
            throw new NotImplementedException();
        }

        public string doExitDump()
        {
            LocalDataDumper dumper = new LocalDataDumper();
            string dumppath = dumper.build(ctcache).dump();
            return dumppath;
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private CTableCache ctcache = null;
    }
}
