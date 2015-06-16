using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.BGHandlerManager;
using System.Windows.Forms;
using IDCM.Core;
using IDCM.MsgDriver;
using IDCM.DataTransfer;
using IDCM.ComPO;

namespace IDCM.BGHandler
{
    class GCMDetailLoadHandler: AbsBGHandler
    {
        public GCMDetailLoadHandler(GCMTableCache gtcache, int ridx, AuthInfo authInfo)
        {
            this.gtcache = gtcache;
            this.rowIndex = ridx;
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
            try{
                DCMPublisher.noteJobProgress(0);
                string sid = null;
                if (gtcache.overViewRowIndexValid(rowIndex) && !gtcache.overViewFocusRecently(rowIndex))
                {
                    sid = gtcache.getSIDByRowIdx(rowIndex);
                    if (sid != null && sid.Length > 0)
                    {
                        res = GCMItemsLoader.loadDetailViewData(gtcache, sid,authInfo);
                        if (res)
                        {
                            DCMPublisher.noteJobFeedback(AsyncMsgNotice.GCMDataLoaded);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to query the detail of GCM data") + "！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to query the detail of GCM data") + "！ " + ex.Message, DCMMsgType.Tip);
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
        private int rowIndex;
        private AuthInfo authInfo;
    }
}
