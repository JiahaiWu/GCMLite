using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Core;
using IDCM.MsgDriver;
using System.Windows.Forms;
using System.ComponentModel;
using IDCM.DataTransfer;
using System.Data;
using IDCM.BGHandlerManager;

namespace IDCM.BGHandler
{
    class ExcelExportHandler:AbsBGHandler
    {
        public ExcelExportHandler(CTableCache ctcache, string xlsPath)
        {
            this.ctcache = ctcache;
            this.xlsPath = System.IO.Path.GetFullPath(xlsPath);
        }
        public ExcelExportHandler(CTableCache ctcache, string xlsPath, int[] selectedRows)
        {
            this.ctcache = ctcache;
            this.xlsPath = System.IO.Path.GetFullPath(xlsPath);
            this.selectedRows = selectedRows;
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
                if (LocalDataChecker.checkForExport(ctcache, 10))
                {
                    ExcelExporter exporter = new ExcelExporter();
                    if (selectedRows != null)
                        res = exporter.exportExcel(ctcache, xlsPath, selectedRows);
                    else
                        res = exporter.exportExcel(ctcache, xlsPath);
                    if (!res)
                    {
                        log.Info(IDCM.Base.GlobalTextRes.Text("Export failed") + ". @filepath=" + xlsPath);
                    }
                    else
                    {
                        log.Info(IDCM.Base.GlobalTextRes.Text("Export success") + ". @filepath=" + xlsPath);
                        DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataExported);
                    }
                }
                else
                {
                    log.Info(IDCM.Base.GlobalTextRes.Text("Data Check failed") + "!");
                    DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Data Check failed") + "!", DCMMsgType.Alert);
                }
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to export XML document")+"！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to export XML document") + "！ " + ex.Message, DCMMsgType.Alert);
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
            
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                log.Info(IDCM.Base.GlobalTextRes.Text("Export failed")+"! @filepath=" + xlsPath);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to export XML document") + "！ " + error.Message, DCMMsgType.Alert);
            }
        }

        private string xlsPath = null;
        private int[] selectedRows;
        private CTableCache ctcache;
    }
}
