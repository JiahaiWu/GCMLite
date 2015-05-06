using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.ComPO;
using IDCM.MsgDriver;
using IDCM.Base;
using IDCM.DataTransfer;
using IDCM.Core;
using IDCM.ComPO;
using IDCM.BGHandler;
using IDCM.BGHandlerManager;

namespace IDCM.ViewManager
{
    class GCMServManager
    {
        public GCMServManager(GCMTableCache gtcache)
        {
            this.gtcache = gtcache;
            signMonitor = new System.Windows.Forms.Timer();
            signMonitor.Interval = SysConstants.SessionValidMilliSeconds / 2;
            signMonitor.Tick += OnSignInHold;
            signMonitor.Start();
            authInfo = new AuthInfo();
        }
        /// <summary>
        /// 用于外部请求式的GCM连接调用方法
        /// </summary>
        internal void connnectGCM()
        {
            authInfo.Username = gtcache.UserName;
            authInfo.Password = gtcache.Password;
            //authInfo.autoLogin = gtcache.RememberLogin;//@depercated
            authInfo.autoLogin = true;
            authInfo.Timestamp = 0;
            new System.Threading.Thread(delegate() { OnSignInHold(null, null); }).Start();
        }

        /// <summary>
        /// 登录状态验证与保持
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSignInHold(object sender, EventArgs e)
        {
            try
            {
                if (authInfo == null)
                    return;
                if (!Signed)
                {
                    if (authInfo != null && authInfo.Username != null && authInfo.Password != null)
                    {
                        DCMPublisher.noteSimpleMsg("正在连接GCM ...", DCMMsgType.Status);
                        AuthInfo auth = GCMSignExecutor.SignIn(authInfo.Username, authInfo.Password, authInfo.autoLogin);
                        authInfo.LoginFlag = auth.LoginFlag;
                        authInfo.Jsessionid = auth.Jsessionid;
                        authInfo.Timestamp = auth.Timestamp;
                    }
                }
                string tip = authInfo.LoginFlag ? "已建立连接. [" + authInfo.Username + "]" : "未连接";
                DCMPublisher.noteSimpleMsg(tip, DCMMsgType.Status);
                DCMPublisher.noteJobFeedback(AsyncMsgNotice.GCMUserSigned);
            }
            catch (Exception ex)
            {
                log.Error("连接到GCM失败", ex);
                DCMPublisher.noteSimpleMsg("连接失败.", DCMMsgType.Status);
            }
        }
        internal bool Signed
        {
            get
            {
                if (authInfo == null || !authInfo.LoginFlag)
                    return false;
                long elapsedTicks = DateTime.Now.Ticks - authInfo.Timestamp;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                return elapsedSpan.TotalMilliseconds < SysConstants.SessionValidMilliSeconds;
            }
        }
        public string UserName
        {
            get
            {
                return Signed ? authInfo.Username:null;
            }
        }

        public string Password 
        { 
            get 
            {
                return Signed ? authInfo.Password:null;
            }
        }

        internal void refreshGCMDataset()
        {
            GCMDataLoadHandler gdlh = new GCMDataLoadHandler(gtcache,authInfo);
            BGWorkerInvoker.pushHandler(gdlh);
        }
        internal void showGCMDataDetail(int ridx=0)
        {
            if (Signed)
            {
                GCMDetailLoadHandler gdlh = new GCMDetailLoadHandler(gtcache, ridx, authInfo);
                BGWorkerInvoker.pushHandler(gdlh);
            }
        }
        private GCMTableCache gtcache;
        /// <summary>
        /// SignIn hold Monitor
        /// </summary>
        private System.Windows.Forms.Timer signMonitor = null;
        /// <summary>
        /// 用户登录信息
        /// </summary>
        private readonly AuthInfo authInfo;
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
