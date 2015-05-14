using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Core;
using System.IO;
using IDCM.Base;
using IDCM.MsgDriver;
using IDCM.DataTransfer;
using IDCM.BGHandlerManager;

namespace IDCM.BGHandler
{
    class GCMTextExportHandler : AbsBGHandler
    {
        public GCMTextExportHandler(GCMTableCache ctableCache, string fpath, string spliter = " ")
        {
            this.textPath = System.IO.Path.GetFullPath(fpath);
            this.spliter = spliter;
            this.ctableCache = ctableCache;
        }

        public GCMTextExportHandler(GCMTableCache ctableCache, string fpath, int[] selectedRows, string spliter = "")
        {
            this.ctableCache = ctableCache;
            this.textPath = System.IO.Path.GetFullPath(fpath);
            this.selectedRows = selectedRows;
            this.spliter = spliter;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(bool cancel, List<Object> args)
        {
            bool res = false;
            try
            {
                DCMPublisher.noteJobProgress(0);
                GCMTextExporter exporter = new GCMTextExporter();
                if (selectedRows != null)
                    res = exporter.exportText(ctableCache, textPath, selectedRows, spliter);
                else
                    res = exporter.exportText(ctableCache, textPath, spliter);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to export text file")+"！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to export text file") + "！ " + ex.Message, DCMMsgType.Alert);
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
            DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataExported);
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                log.Info(IDCM.Base.GlobalTextRes.Text("Export failed")+"! @filepath=" + textPath);
            }
            else
            {
                log.Info(IDCM.Base.GlobalTextRes.Text("Export success") + ". @filepath=" + textPath);
            }
        }

        private string spliter = null;
        private string textPath = null;
        private GCMTableCache ctableCache = null;
        private int[] selectedRows;
    }
}
