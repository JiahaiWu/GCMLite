using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Base.AbsInterfaces;
using IDCM.MsgDriver;
using IDCM.Core;
using System.Collections;

namespace IDCM.BGHandler
{
    class LocalDataFilterToRecvHandler : AbsBGHandler
    {
        private Core.CTableCache ctcache;
        private ICollection selectedRows;

        public LocalDataFilterToRecvHandler(Core.CTableCache ctcache)
        {
            this.ctcache = ctcache;
        }

        public LocalDataFilterToRecvHandler(Core.CTableCache ctcache, DataGridViewSelectedRowCollection selectedRows)
        {
            this.ctcache = ctcache;
            this.selectedRows = selectedRows;
        }

        public LocalDataFilterToRecvHandler(Core.CTableCache ctcache, DataGridViewRow[] selectedRows)
        {
            this.ctcache = ctcache;
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
                if (selectedRows != null)
                    res = LocalDataChecker.tryRecover(ctcache, selectedRows);
                else
                    res = LocalDataChecker.tryRecover(ctcache);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Local data validation did not pass")+"！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: "+IDCM.Base.GlobalTextRes.Text("Local data validation did not pass")+"！ " + ex.Message, IDCM.Base.ComPO.DCMMsgType.Alert);
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
            DCMPublisher.noteJobFeedback(Base.ComPO.AsyncMsgNotice.LocalDataChecked);
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                log.Info(IDCM.Base.GlobalTextRes.Text("Data filter failed")+"!");
                DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Data filter failed") + "!", Base.ComPO.DCMMsgType.Tip);
            }
            else
            {
                log.Info(IDCM.Base.GlobalTextRes.Text("Data filter success")+".");
                DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Data filter success") + ".", Base.ComPO.DCMMsgType.Tip);
            }
        }
    }
}
