using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.ComPO;
using IDCM.Base.AbsInterfaces;
using System.Threading;
using System.Collections.Concurrent;
using IDCM.Base;
using System.Windows.Forms;

namespace IDCM.MsgDriver
{
    public class DCMPublisher
    {
        #region Methods
        public static void noteJobProgress(Int32 percent)
        {
            if (msgObs == null)
                throw new IDCMException("Default MsgObserver hasn't inited for publish Message!");
            sendMsgCache.Enqueue(percent);
            messageMonitor.Enabled = true;
        }
        public static void noteSimpleMsg(DCMMessage dmsg)
        {
            if (msgObs == null)
                throw new IDCMException("Default MsgObserver hasn't inited for publish Message!");
            sendMsgCache.Enqueue(dmsg);
            messageMonitor.Enabled = true;
        }
        public static void noteSimpleMsg(string msg, DCMMsgType msgType = DCMMsgType.Tip)
        {
            if (msgObs == null)
                throw new IDCMException("Default MsgObserver hasn't inited for publish Message!");
            sendMsgCache.Enqueue(new DCMMessage(msgType, msg));
            messageMonitor.Enabled = true;
        }
        public static void noteJobFeedback(AsyncMsgNotice amsg)
        {
            if (msgObs == null)
                throw new IDCMException("Default MsgObserver hasn't inited for publish Message!");
            sendMsgCache.Enqueue(amsg);
            messageMonitor.Enabled = true;
        }
        public static IMsgObserver initDefaultMsgObserver()
        {
            msgObs=new MsgObserver();
            sendMsgCache = new ConcurrentQueue<object>();
            messageMonitor = new System.Timers.Timer();
            messageMonitor.Interval = 5;
            messageMonitor.Elapsed += OnMMHeartBreak;
            messageMonitor.Enabled = false;
            return msgObs;
        }
        #endregion

        #region Events&Handlings
        /// <summary>
        /// 异步消息轮询监视器的心跳检测事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnMMHeartBreak(object sender, EventArgs e)
        {
            try
            {
                object sendPair = null;
                if (msgObs == null)
                    throw new IDCMException("Default MsgObserver hasn't inited for publish Message!");
                ICollection<IMsgListener> listeners = msgObs.getMsgListeners();
                while (sendMsgCache.TryDequeue(out sendPair))
                {
                    foreach (IMsgListener ls in listeners)
                    {
                        string srcType = sendPair.GetType().FullName;
                        if (srcType.Equals(typeof(Int32).FullName))
                            ls.reportJobProgress((Int32)sendPair);
                        else if (srcType.Equals(typeof(DCMMessage).FullName))
                            ls.reportSimpleMsg((DCMMessage)sendPair);
                        else if (srcType.Equals(typeof(AsyncMsgNotice).FullName))
                            ls.reportJobFeedback((AsyncMsgNotice)sendPair);
                    }
                }
                messageMonitor.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion

        #region Property
        public static IMsgObserver MsgObs
        {
            get
            {
                return msgObs;
            }
        }
        #endregion

        #region Members
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static MsgObserver msgObs=null;
        /// <summary>
        /// Message Instance Monitor
        /// </summary>
        private static System.Timers.Timer messageMonitor = null;
        /// <summary>
        /// send Message Cache
        /// </summary>
        private static ConcurrentQueue<object> sendMsgCache = null;
        #endregion
    }
}
