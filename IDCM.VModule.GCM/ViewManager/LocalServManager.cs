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
            int[] selectedRowIdxs = fetchSelectRowIdxs(dgv);
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
                            handler = selectedRowIdxs == null ? new ExcelExportHandler(ctcache, epath) : new ExcelExportHandler(ctcache, epath,selectedRowIdxs);
                            break;
                        case ExportType.JSONList:
                            handler = selectedRowIdxs == null ? new JSONListExportHandler(ctcache, epath):new JSONListExportHandler(ctcache, epath,selectedRowIdxs);
                            break;
                        case ExportType.TSV:
                            handler = selectedRowIdxs == null ? new TextExportHandler(ctcache, epath, "\t"):new TextExportHandler(ctcache, epath, selectedRowIdxs,"\t");
                            break;
                        case ExportType.CSV:
                            handler = selectedRowIdxs == null ? new TextExportHandler(ctcache, epath, ","):new TextExportHandler(ctcache, epath,selectedRowIdxs, ",");
                            break;
                        case ExportType.XML:
                            handler = selectedRowIdxs == null ? new XMLExportHandler(ctcache, epath) : new XMLExportHandler(ctcache, epath,selectedRowIdxs);
                            break;
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

        private int[] fetchSelectRowIdxs(DataGridView dgv)
        {
            DataGridViewSelectedRowCollection selectedRows=dgv.SelectedRows;
            if (selectedRows != null && selectedRows.Count > 0)
            {
                int[] sridxs = new int[selectedRows.Count];
                int idx = 0;
                foreach (DataGridViewRow dgvr in selectedRows)
                {
                    sridxs[idx] = dgvr.Index;
                }
                return sridxs;
            }
            return null;
        }

        /// <summary>
        /// 校验数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void checkLocalData()
        {
            AbsBGHandler checkHandler = new LocalDataCheckHandler(ctcache);
            BGWorkerInvoker.pushHandler(checkHandler);
        }
        public void checkLocalData(DataGridViewSelectedRowCollection selectedRows)
        {
            AbsBGHandler checkHandler = new LocalDataCheckHandler(ctcache, selectedRows);
            BGWorkerInvoker.pushHandler(checkHandler);
        }
        public void checkData(params DataGridViewRow[] selectedRows)
        {
            AbsBGHandler checkHandler = new LocalDataCheckHandler(ctcache, selectedRows);
            BGWorkerInvoker.pushHandler(checkHandler);
        }

        internal void publishLocalDataToGCM()
        {
            AbsBGHandler checkHandler = new LocalDataPubHandler(ctcache);
            BGWorkerInvoker.pushHandler(checkHandler);
        }
        internal void publishLocalDataToGCM(params DataGridViewRow[] selectedRows)
        {
            AbsBGHandler checkHandler = new LocalDataPubHandler(ctcache, selectedRows);
            BGWorkerInvoker.pushHandler(checkHandler);
        }

        internal void publishLocalDataToGCM(DataGridViewSelectedRowCollection selectedRows)
        {
            AbsBGHandler checkHandler = new LocalDataPubHandler(ctcache, selectedRows);
            BGWorkerInvoker.pushHandler(checkHandler);
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
