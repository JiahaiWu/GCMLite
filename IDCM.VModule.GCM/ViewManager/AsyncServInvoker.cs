using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.AbsInterfaces;
using IDCM.Base.ComPO;

namespace IDCM.ViewManager
{
    public class AsyncServInvoker:IMsgListener
    {
        public AsyncServInvoker()
        {
            this.OnSimpleMsgAlert += _OnSimpleMsgAlert;
            this.OnSimpleMsgTrace += _OnSimpleMsgTrace;
        }
        #region GCM Pro 异步消息事务分发处理
        /// <summary>
        /// 消息事件分发处理
        /// </summary>
        /// <param name="msg"></param>
        public void reportJobProgress(Int32 percent)
        {
            if (OnProgressChange!=null)
            {
                if (percent < 1 || percent > 99)
                {
                    OnProgressChange(true);
                }
                else
                {
                    OnProgressChange(!IDCM.BGHandlerManager.RunningHandlerNoter.checkForIdle());
                }
            }
        }
        /// <summary>
        /// 消息事件分发处理
        /// </summary>
        /// <param name="msg"></param>
        public void reportSimpleMsg(DCMMessage dmsg)
        {
            if (dmsg == null)
                return;
            switch (dmsg.MsgType)
            {
                case DCMMsgType.Alert:
                    if (OnSimpleMsgAlert != null)
                        OnSimpleMsgAlert(dmsg.Msg);
                    break;
                case DCMMsgType.Tip:
                    if (OnSimpleMsgTip != null)
                        OnSimpleMsgTip(dmsg.Msg);
                    break;
                case DCMMsgType.Trace:
                    if (OnSimpleMsgTrace != null)
                        OnSimpleMsgTrace(dmsg.Msg);
                    break;
                case DCMMsgType.Status:
                    if(OnBottomSatusChange!=null)
                        OnBottomSatusChange(dmsg.Msg);
                    break;
            }
        }
        /// <summary>
        /// 消息事件分发处理
        /// </summary>
        /// <param name="msg"></param>
        public void reportJobFeedback(AsyncMsgNotice amsg)
        {
            if (amsg == null)
                return;
            switch (amsg.MsgType)
            {
                case MsgNoticeType.GCMDataLoaded:
                    if(OnGCMDataLoaded!=null)
                        OnGCMDataLoaded( amsg.MsgTag, amsg.Parameters);
                    break;
                case MsgNoticeType.LocalDataExported:
                    if (OnLocalDataExported != null)
                        OnLocalDataExported(amsg.MsgTag, amsg.Parameters);
                    break;
                case MsgNoticeType.LocalDataImported:
                    if (OnLocalDataImported != null)
                        OnLocalDataImported(amsg.MsgTag, amsg.Parameters);
                    break;
                default:
                    log.Warn("Unhandled asynchronous message.  @msgTag=" + amsg.MsgTag);
                    break;
            }
        }

        //定义数据源加载完成事件

        internal event IDCMAsyncRequest OnSimpleMsgTip;
        internal event IDCMAsyncRequest OnSimpleMsgAlert;
        internal event IDCMAsyncRequest OnSimpleMsgTrace;

        internal event IDCMAsyncRequest OnBottomSatusChange;
        internal event IDCMAsyncRequest OnProgressChange;
        internal event IDCMAsyncRequest OnGCMDataLoaded;
        internal event IDCMAsyncRequest OnLocalDataExported;
        internal event IDCMAsyncRequest OnLocalDataImported;
        #endregion
        #region 默认的消息事件处理行为定义
        private void _OnSimpleMsgAlert(object msgTag, params object[] vals)
        {
            log.Info(msgTag.ToString());
            System.Windows.Forms.MessageBox.Show(msgTag.ToString());
        }
        private void _OnSimpleMsgTrace(object msgTag, params object[] vals)
        {
            log.Info(msgTag.ToString());
        }
        #endregion

        //异步消息事件委托形式化声明
        public delegate void IDCMAsyncRequest(object msgTag, params object[] vals);
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
