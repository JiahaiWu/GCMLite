using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Base.AbsInterfaces;
using IDCM.Core;
using IDCM.MsgDriver;
using IDCM.DataTransfer;

namespace IDCM.BGHandler
{
    class GCMExcelExportHandler:AbsBGHandler
    {
        public GCMExcelExportHandler(GCMTableCache gtcache, string xlsPath, bool exportStrainTree=false)
        {
            this.gtcache = gtcache;
            this.xlsPath = System.IO.Path.GetFullPath(xlsPath);
            this.exportDetail = exportStrainTree;
        }
        public GCMExcelExportHandler(GCMTableCache gtcache, string xlsPath, int[] selectedRows, bool exportStrainTree=false)
        {
            this.gtcache = gtcache;
            this.xlsPath = System.IO.Path.GetFullPath(xlsPath);
            this.selectedRows = selectedRows;
            this.exportDetail = exportStrainTree;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(bool cancel, List<Object> args)
        {
            bool res = false;
            try{
                DCMPublisher.noteJobProgress(0);
                GCMExcelExporter exporter = new GCMExcelExporter();
                if (selectedRows != null)
                    res = exporter.exportExcel(gtcache, xlsPath, selectedRows,exportDetail);
                else
                    res = exporter.exportExcel(gtcache, xlsPath, exportDetail);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to export XML document")+"！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to export XML document") + "！ " + ex.Message, IDCM.Base.ComPO.DCMMsgType.Alert);
            }
            return new object[] { res };
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override void complete(bool canceled, Exception error, List<Object> args)
        {
            DCMPublisher.noteJobProgress(100);
            DCMPublisher.noteJobFeedback(Base.ComPO.AsyncMsgNotice.LocalDataExported);
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                log.Info(IDCM.Base.GlobalTextRes.Text("Export failed")+"! @filepath=" + xlsPath);
            }
            else
            {
                log.Info(IDCM.Base.GlobalTextRes.Text("Export success") + ". @filepath=" + xlsPath);
            }
        }

        private string xlsPath = null;
        private int[] selectedRows;
        private GCMTableCache gtcache;
        private bool exportDetail;
    }
}
