using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Core;
using IDCM.MsgDriver;
using IDCM.Base.Utils;
using Newtonsoft.Json;
using IDCM.DataTransfer;
using IDCM.BGHandlerManager;

namespace IDCM.BGHandler
{
    class MDIImportHandler : AbsBGHandler
    {
        public MDIImportHandler(CTableCache ctcache, string fpath)
        {
            this.ctcache = ctcache;
            this.mdiPath = System.IO.Path.GetFullPath(fpath);
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override object doWork(bool cancel, List<object> args)
        {
            bool res = false;
            try
            {
                DCMPublisher.noteJobProgress(0);
                res = MDIDataImporter.parseMDIData(ctcache, mdiPath);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to import dump file") + "！", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import dump file") + "！ " + ex.Message, DCMMsgType.Alert);
            }
            return new object[] { res, mdiPath };
        }

        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override void complete(bool canceled, Exception error, List<Object> args)
        {
            DCMPublisher.noteJobProgress(100);
            DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataImported);
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                return;
            }
        }
        public override void addHandler(AbsBGHandler nextHandler)
        {
            base.addHandler(nextHandler);
        }
        private CTableCache ctcache;
        
        private string mdiPath = null;
    }
}
