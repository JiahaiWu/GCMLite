using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDCM.Core;
using IDCM.MsgDriver;
using IDCM.DataTransfer;
using IDCM.ComPO;
using IDCM.BGHandlerManager;

namespace IDCM.BGHandler
{
    class GCMDataLoadHandler: AbsBGHandler
    {
        public GCMDataLoadHandler(GCMTableCache gtcache,AuthInfo authInfo)
        {
            this.gtcache = gtcache;
            this.authInfo = authInfo;
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
                res = GCMItemsLoader.loadOverViewData(gtcache, authInfo);
                if (res)
                {
                    DCMPublisher.noteJobFeedback(AsyncMsgNotice.GCMDataLoaded);
                }
            }
            catch (Exception ex)
            {
                log.Info("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to query the overview of GCM data") + "！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to query the overview of GCM data") + "！ " + ex.Message, DCMMsgType.Tip);
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
                return;
            }
        }

        private GCMTableCache gtcache = null;
        private AuthInfo authInfo = null;
    }
}
