﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.ComPO;
using IDCM.Base;
using IDCM.Base.Utils;
using System.IO;
using System.Configuration;
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
using IDCM.ComPO;

namespace IDCM.ViewManager
{
    class LocalServManager
    {
        #region Constructor&Destructor
        public LocalServManager(CTableCache ctcache)
        {
            this.ctcache = ctcache;
            lastIOPath=ConfigurationManager.AppSettings[SysConstants.LastWorkSpace];
        }
        #endregion

        #region Methods

        internal int addNewRow()
        {
            Dictionary<string, string> mapvalues = new Dictionary<string, string>();
            foreach (CustomColDef ccd in CustomColDefGetter.getCustomTableDef())
            {
                mapvalues[ccd.Attr] = ccd.DefaultVal;
            }
            int ridx = -1;
            lock (ctcache.GSyncRoot)
            {
                ridx=ctcache.addRow(mapvalues);
            }
            return ridx;
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
            if (fpath.ToLower().EndsWith(".gcms"))
            {
                if (DataImportChecker.checkForMDIImport(fpath))
                {
                    eih = new MDIImportHandler(ctcache, fpath);
                }
            }
            if (fpath.ToLower().EndsWith(".tsv")|| fpath.ToLower().EndsWith(".csv"))
            {
                ExportType txtType=fpath.ToLower().EndsWith(".tsv") ? ExportType.TSV : ExportType.CSV;
                if (DataImportChecker.checkForTextImport(fpath, ref dataMapping, txtType))
                {
                    eih = new TextImportHandler(ctcache, fpath, txtType, ref dataMapping);
                }
            }
            if (fpath.ToLower().EndsWith(".jso"))
            {
                if (DataImportChecker.checkForJSOImport(fpath, ref dataMapping))
                {
                    eih = new JSONListImportHandler(ctcache, fpath, ref dataMapping);
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
            ExportTypeDlg exportDlg = new ExportTypeDlg(lastIOPath);
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
                            MessageBox.Show(GlobalTextRes.Text("Unsupport export type")+"!");
                            break;
                    }
                    if (handler != null)
                    {
                        lastIOPath = epath;
                        BGWorkerInvoker.pushHandler(handler);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GlobalTextRes.Text("Data export failed"));
                    log.Info(GlobalTextRes.Text("Data export failed with error info") + "：", ex);
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

        internal void removeRow(DataGridViewRow dgvr)
        {
            lock (ctcache.GSyncRoot)
            {
                ctcache.removeRow(dgvr);
            }
        }
        internal void syncKeyCellValue(DataGridViewRow dgvr)
        {
            dgvr.Tag = null;
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
        internal void filterToRecvLocalData()
        {
            AbsBGHandler checkHandler = new LocalDataFilterToRecvHandler(ctcache);
            BGWorkerInvoker.pushHandler(checkHandler);
        }
        public void checkData(params DataGridViewRow[] selectedRows)
        {
            AbsBGHandler checkHandler = new LocalDataCheckHandler(ctcache, selectedRows);
            BGWorkerInvoker.pushHandler(checkHandler);
        }

        internal void publishLocalDataToGCM(AuthInfo authInfo)
        {
            AbsBGHandler checkHandler = new LocalDataPubHandler(authInfo,ctcache);
            BGWorkerInvoker.pushHandler(checkHandler);
        }
        internal void publishLocalDataToGCM(AuthInfo authInfo,params DataGridViewRow[] selectedRows)
        {
            AbsBGHandler checkHandler = new LocalDataPubHandler(authInfo, ctcache, selectedRows);
            BGWorkerInvoker.pushHandler(checkHandler);
        }

        internal void publishLocalDataToGCM(AuthInfo authInfo,DataGridViewSelectedRowCollection selectedRows)
        {
            AbsBGHandler checkHandler = new LocalDataPubHandler(authInfo, ctcache, selectedRows);
            BGWorkerInvoker.pushHandler(checkHandler);
        }

        public string doDumpWork()
        {
            if (lastIOPath != null && lastIOPath.Length > 0)
            {
                ConfigurationHelper.SetAppConfig(SysConstants.LastWorkSpace, lastIOPath, SysConstants.defaultCfgPath);
            }
            LocalDataDumper dumper = new LocalDataDumper();
            string dumppath = dumper.build(ctcache).dump();
            return dumppath;
        }

        #endregion

        #region Property
        public int RowCount
        {
            get
            {
                return ctcache.getRowCount();
            }
        }
        public int KeyColIndex
        {
            get
            {
                return ctcache.getKeyColIndex();
            }
        }
        public string LastIOPath
        {
            get
            {
                if (lastIOPath == null)
                {
                    lastIOPath = System.IO.Directory.GetCurrentDirectory();
                }
                else
                    lastIOPath = Path.GetDirectoryName(lastIOPath);
                if (!Directory.Exists(lastIOPath))
                    lastIOPath = System.IO.Directory.GetCurrentDirectory();
                return lastIOPath;
            }
            set
            {
                lastIOPath = value;
            }
        }
        #endregion

        #region Members
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private CTableCache ctcache = null;
        private string lastIOPath = null;
        #endregion


    }
}
