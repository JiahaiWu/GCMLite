using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Base.AbsInterfaces;
using IDCM.MsgDriver;
using IDCM.Core;

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
            }
            catch (Exception ex)
            {
                log.Error("本地数据验证失败！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: 本地数据验证失败！ " + ex.Message, IDCM.Base.ComPO.DCMMsgType.Alert);
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
                log.Info("Data Check Failed!");
                DCMPublisher.noteSimpleMsg("Data Check Failed!", Base.ComPO.DCMMsgType.Tip);
            }
            else
            {
                log.Info("Data Check success.");
                DCMPublisher.noteSimpleMsg("Data Check success.", Base.ComPO.DCMMsgType.Tip);
            }
        }
    }
}
