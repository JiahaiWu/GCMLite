using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.MsgDriver;
using IDCM.Core;
using IDCM.BGHandlerManager;

namespace IDCM.BGHandler
{
    class LocalDataCheckHandler:AbsBGHandler
    {
        private Core.CTableCache ctcache;
        private ICollection selectedRows;

        public LocalDataCheckHandler(Core.CTableCache ctcache)
        {
            this.ctcache = ctcache;
        }

        public LocalDataCheckHandler(Core.CTableCache ctcache, DataGridViewSelectedRowCollection selectedRows)
        {
            this.ctcache = ctcache;
            this.selectedRows = selectedRows;
        }

        public LocalDataCheckHandler(Core.CTableCache ctcache,DataGridViewRow[] selectedRows)
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
                    res = LocalDataChecker.checkForExport(ctcache, selectedRows);
                else
                    res = LocalDataChecker.checkForExport(ctcache);
                if(res)
                    DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataChecked);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Local data validation did not pass")+"！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: "+IDCM.Base.GlobalTextRes.Text("Local data validation did not pass")+"！ " + ex.Message, DCMMsgType.Alert);
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
                log.Info(IDCM.Base.GlobalTextRes.Text("Data Check failed")+"!");
                DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Data Check failed") + "!", DCMMsgType.Tip);
            }
            else
            {
                log.Info(IDCM.Base.GlobalTextRes.Text("Data Check success")+".");
                DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Data Check success")+".", DCMMsgType.Tip);
            }
        }
    }
}
