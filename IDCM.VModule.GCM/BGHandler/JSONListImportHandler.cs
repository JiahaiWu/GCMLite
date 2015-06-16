using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Core;
using IDCM.MsgDriver;
using IDCM.BGHandlerManager;
using IDCM.DataTransfer;

namespace IDCM.BGHandler
{
    class JSONListImportHandler: AbsBGHandler
    {
        public JSONListImportHandler(CTableCache ctcache, string fpath, ref Dictionary<string, string> dataMapping)
        {
            this.ctcache = ctcache;
            this.dataMapping = dataMapping;
            this.jsoPath = System.IO.Path.GetFullPath(fpath);
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
                res = JSONListDataImporter.parseJSONListData(ctcache, jsoPath,ref dataMapping);
                if(res)
                    DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataImported);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to import JSON list file") + "！", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import JSON list file") + "！ " + ex.Message, DCMMsgType.Alert);
            }
            return new object[] { res, jsoPath };
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
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import JSON list file") + "！ " + error.Message, DCMMsgType.Alert);
                return;
            }
        }
        public override void addHandler(AbsBGHandler nextHandler)
        {
            base.addHandler(nextHandler);
        }
        private CTableCache ctcache;
        
        private string jsoPath = null;
        private Dictionary<string, string> dataMapping = null;
    }
}
