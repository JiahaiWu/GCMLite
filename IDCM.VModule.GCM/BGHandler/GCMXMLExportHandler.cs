using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Core;
using IDCM.Base;
using System.IO;
using System.Xml;
using IDCM.Base.AbsInterfaces;
using IDCM.MsgDriver;
using IDCM.DataTransfer;

namespace IDCM.BGHandler
{
    class GCMXMLExportHandler : AbsBGHandler
    {
        public GCMXMLExportHandler(GCMTableCache ctcache, string fpath)
        {
            this.textPath = System.IO.Path.GetFullPath(fpath);
            this.ctcache = ctcache;
        }

        public GCMXMLExportHandler(GCMTableCache ctcache, string fpath, int[] selectedRows)
        {
            this.ctcache = ctcache;
            this.textPath = System.IO.Path.GetFullPath(fpath); ;
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
            try
            {
                DCMPublisher.noteJobProgress(0);
                GCMXMLExporter exporter = new GCMXMLExporter();
                if (selectedRows != null)
                    res = exporter.exportXML(ctcache, textPath, selectedRows);
                else
                    res = exporter.exportXML(ctcache, textPath);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to import XML document")+"！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import XML document") + "！ " + ex.Message, IDCM.Base.ComPO.DCMMsgType.Alert);
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
                log.Info(IDCM.Base.GlobalTextRes.Text("Export failed") + "! @filepath=" + textPath);
            }
            else
            {
                log.Info(IDCM.Base.GlobalTextRes.Text("Export success") + ". @filepath=" + textPath);
            }
        }

        private string textPath = null;
        private GCMTableCache ctcache = null;
        private int[] selectedRows;
    }
}
