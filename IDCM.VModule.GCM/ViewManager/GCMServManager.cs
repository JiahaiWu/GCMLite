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
using IDCM.Forms;
using System.Windows.Forms;

namespace IDCM.ViewManager
{
    class GCMServManager
    {
        #region Constructor&Destructor
        public GCMServManager(GCMTableCache gtcache)
        {
            this.gtcache = gtcache;
            signMonitor = new System.Windows.Forms.Timer();
            signMonitor.Interval = SysConstants.SessionValidMilliSeconds / 2;
            signMonitor.Tick += OnSignInHold;
            signMonitor.Start();
            authInfo = new AuthInfo();
        }
        #endregion

        #region Methods
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
                        DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Connecting to GCM") + " ...", DCMMsgType.Status);
                        AuthInfo auth = GCMSignExecutor.SignIn(authInfo.Username, authInfo.Password, authInfo.autoLogin);
                        authInfo.LoginFlag = auth.LoginFlag;
                        authInfo.Jsessionid = auth.Jsessionid;
                        authInfo.Timestamp = auth.Timestamp;
                    }
                }
                string tip = authInfo.LoginFlag ? GlobalTextRes.Text("Connected") + ". [" + authInfo.Username + "]" : GlobalTextRes.Text("Disconnected");
                DCMPublisher.noteSimpleMsg(tip, DCMMsgType.Status);
                DCMPublisher.noteSimpleMsg(tip);
                DCMPublisher.noteJobFeedback(AsyncMsgNotice.GCMUserSigned);
            }
            catch (Exception ex)
            {
                log.Error(GlobalTextRes.Text("Connect GCM failed"), ex);
                DCMPublisher.noteSimpleMsg(GlobalTextRes.Text("Connect GCM failed"), DCMMsgType.Status);
                DCMPublisher.noteSimpleMsg(GlobalTextRes.Text("Connect GCM failed"));
            }
        }

        internal AuthInfo getAuthInfo()
        {
            return this.authInfo;
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

        internal void downGCMData(DataGridView dgv)
        {
            int[] selectedRowIdxs = fetchSelectRowIdxs(dgv);
            GCMExportTypeDlg exportDlg = new GCMExportTypeDlg();
            if (exportDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    AbsBGHandler handler = null;
                    ExportType etype = GCMExportTypeDlg.LastOptionValue;
                    string epath = GCMExportTypeDlg.LastFilePath;
                    bool exportStrainTree = GCMExportTypeDlg.ExportStainTree;
                    switch (etype)
                    {
                        case ExportType.Excel:
                            handler = selectedRowIdxs == null ? new GCMExcelExportHandler(gtcache, epath) : new GCMExcelExportHandler(gtcache, epath, selectedRowIdxs);
                            break;
                        case ExportType.JSONList:
                            handler = selectedRowIdxs == null ? new GCMJSONListExportHandler(gtcache, epath) : new GCMJSONListExportHandler(gtcache, epath, selectedRowIdxs);
                            break;
                        case ExportType.TSV:
                            handler = selectedRowIdxs == null ? new GCMTextExportHandler(gtcache, epath, "\t") : new GCMTextExportHandler(gtcache, epath, selectedRowIdxs, "\t");
                            break;
                        case ExportType.CSV:
                            handler = selectedRowIdxs == null ? new GCMTextExportHandler(gtcache, epath, ",") : new GCMTextExportHandler(gtcache, epath, selectedRowIdxs, ",");
                            break;
                        case ExportType.XML:
                            handler = selectedRowIdxs == null ? new GCMXMLExportHandler(gtcache, epath) : new GCMXMLExportHandler(gtcache, epath, selectedRowIdxs);
                            break;
                        default:
                            MessageBox.Show(GlobalTextRes.Text("Unsupport export type") + "!");
                            break;
                    }
                    if (handler != null)
                        BGWorkerInvoker.pushHandler(handler);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GlobalTextRes.Text("Data export failed"));
                    log.Info(GlobalTextRes.Text("Data export failed with error info") + "：", ex);
                }
            }
        }
        private int[] fetchSelectRowIdxs(DataGridView dgv)
        {
            DataGridViewSelectedRowCollection selectedRows = dgv.SelectedRows;
            if (selectedRows != null && selectedRows.Count > 0)
            {
                int[] sridxs = new int[selectedRows.Count];
                int idx = 0;
                foreach (DataGridViewRow dgvr in selectedRows)
                {
                    sridxs[idx] = dgvr.Index;
                }
                return sridxs;
            }
            return null;
        }
        #endregion

        #region Property
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
                return Signed ? authInfo.Username : null;
            }
        }

        public string Password
        {
            get
            {
                return Signed ? authInfo.Password : null;
            }
        }
        #endregion

        #region Members
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
        #endregion
    }
}
