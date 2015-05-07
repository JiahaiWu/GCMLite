using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDCM.ViewManager;
using IDCM.Base;
using IDCM.Base.Utils;
using System.Configuration;
using System.IO;
using IDCM.ComponentUtil;
using IDCM.Core;
using DCMControlLib;
using IDCM.BGHandlerManager;
using System.Collections.Concurrent;
using IDCM.ComPO;
using IDCM.DataTransfer;

namespace IDCM.VModule.GCM
{
    /// <summary>
    /// GCMPro定制的用户控件，主体包含三个固定的Tab页签(Local DataSet,GCM Publish,ABC Brwoser)。
    /// 
    /// </summary>
    public partial class GCMProView : UserControl
    {
        public GCMProView()
        {
            checkWorkSpace();
            InitializeComponent();
            InitializeMsgDriver();
            InitializeGCMPro();
            startLocalDataRender();
            startGCMSiteRender();
        }
        private void InitializeMsgDriver()
        {
            log.Debug("InitializeMsgDriver(...)");
            //////////////////////////////////////////////////////
            //初始化消息池
            servInvoker = new AsyncServInvoker();
            Base.AbsInterfaces.IMsgObserver msgObs = MsgDriver.DCMPublisher.initDefaultMsgObserver();
            msgObs.bind(servInvoker);
            ////////////////////////////////////////////////////
            //绑定消息事件处理方法
            servInvoker.OnGCMUserSigned += OnGCMUserSigned;
            servInvoker.OnLocalDataExported += OnLocalDataExported;
            servInvoker.OnLocalDataImported += OnLocalDataImported;
            servInvoker.OnSimpleMsgTip+=OnSimpleMsgTip;
            servInvoker.OnGCMItemDetailRender += OnGCMItemDetailRender;
        }
        /// <summary>
        /// 初始化流程
        /// </summary>
        private void InitializeGCMPro()
        {
            log.Debug("InitializeGCMPro(...)");
            try
            {
                //////////////////////////////////////////////////////
                //加载本地数据表的字段配置
                ICollection<CustomColDef> ccds = CustomColDefGetter.getCustomTableDef();
                ctcache = new CTableCache(dcmDataGridView_local,ccds,ccds.First().Attr);
                ////////////////////////////////////////////////////
                //设定本地数据表属性及事件处理方法
                localServManager = new LocalServManager(ctcache);
                this.dcmDataGridView_local.AllowDrop = true;
                this.dcmDataGridView_local.CellValueChanged += dataGridView_local_CellValueChanged;
                this.dcmDataGridView_local.DragEnter += dataGridView_local_items_DragEnter;
                this.dcmDataGridView_local.DragDrop += dataGridView_local_items_DragDrop;
                this.dcmDataGridView_local.ColumnStateChanged += dataGridView_local_columns_StateChanged;
                this.dcmDataGridView_local.CellMouseClick += dataGridView_local_CustomContextMenuDetect;
                this.dcmDataGridView_local.IsDefaultPasteAble = false;
                this.dcmDataGridView_local.AllowUserToOrderColumns = true;

                this.cellContextMenu_local = new System.Windows.Forms.ContextMenu();
                this.cellContextMenu_local.MenuItems.Add(new MenuItem("Copy", OnLocalCopyClick));
                this.cellContextMenu_local.MenuItems.Add(new MenuItem("Paste", OnLocalPasteClick));
                this.cellContextMenu_local.MenuItems.Add(new MenuItem("Submit record", OnLocalSubmitClick));
                this.cellContextMenu_local.MenuItems.Add(new MenuItem("Search record", OnLocalSearchClick));
                this.dcmDataGridView_local.KeyDown += OnLocalKeyDownDetect;
                //加载GCM发布数据表
                gtcache = new GCMTableCache(textBox_ccinfoId, textBox_pwd, checkBox_remember,dcmDataGridView_gcm, dcmTreeView_gcm);
                gcmServManager = new GCMServManager(gtcache);
                this.dcmDataGridView_gcm.CellClick+=dataGridView_gcm_CellClicked;
                //加载ABC WebKit
                abcServManager = new ABCServManager(abcBrowser_abc);
                ////////////////////////////////////////////////////
                //设置初始化的页签
                gcmTabControl_GCM.SelectedIndex = tabPageEx_Local.TabIndex;
                ////////////////////////////////////////////////////
                this.IsInited = true;
            }
            catch (IDCMException ex)
            {
                this.IsInited = false;
                log.Error("Application View Initialize Failed!", ex);
                MessageBox.Show("Application View Initialize Failed! @Message=" + ex.Message+" \n"+ex.ToString());
            }
            finally
            {
                this.Enabled = this.IsInited;
                this.Visible = this.Enabled;
            }
        }

        private void OnLocalSubmitClick(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dcmDataGridView_local.SelectedRows;
            if (selectedRows != null && selectedRows.Count > 0)
            {
                if (gcmServManager.Signed)
                {
                    localServManager.publishLocalRowsToGCM(selectedRows);
                }
                else
                {
                    MessageBox.Show("请登录GCM后再提交。");
                    //this.gcmTabControl_GCM.SelectedIndex = tabPageEx_gcm.TabIndex;
                }
            }else if (dcmDataGridView_local.CurrentCell != null)
            {
                if (gcmServManager.Signed)
                {
                    localServManager.publishLocalRowsToGCM(dcmDataGridView_local.CurrentRow);
                }
                else
                {
                    MessageBox.Show("请登录GCM后再提交。");
                    //this.gcmTabControl_GCM.SelectedIndex = tabPageEx_gcm.TabIndex;
                }
            }
        }
        private void OnLocalSearchClick(object sender, EventArgs e)
        {
            if (dcmDataGridView_local.CurrentRow != null)
            {
                DataGridViewCellCollection dgvcc = dcmDataGridView_local.CurrentRow.Cells;
                string strainId = dgvcc[ctcache.getKeyColIndex()].FormattedValue.ToString();
                if(abcServManager.linkTo(strainId))
                    gcmTabControl_GCM.SelectedIndex = tabPage_ABC.TabIndex;
            }
        }

        private void OnLocalCopyClick(object sender, EventArgs e)
        {
            DataObject d = this.dcmDataGridView_local.GetClipboardContent();
            Clipboard.SetDataObject(d);
        }

        private void OnLocalPasteClick(object sender, EventArgs e)
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int iFail = 0, iRow = this.dcmDataGridView_local.CurrentCell.RowIndex;
                int iCol = this.dcmDataGridView_local.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                foreach (string line in lines)
                {
                    if (iRow < this.dcmDataGridView_local.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.dcmDataGridView_local.ColumnCount)
                            {
                                oCell = this.dcmDataGridView_local[iCol + i, iRow];
                                if (!oCell.ReadOnly)
                                {
                                    if (oCell.Value == null || oCell.Value.ToString() != sCells[i])
                                    {
                                        oCell.Value = Convert.ChangeType(sCells[i], oCell.ValueType);
                                        oCell.Style.BackColor = Color.Tomato;
                                    }
                                    else
                                        iFail++;//only traps a fail if the data has changed and you are pasting into a read only cell
                                }
                            }
                            else
                            { break; }
                        }
                        iRow++;
                    }
                    else
                    { break; }
                    if (iFail > 0)
                        MessageBox.Show(string.Format("{0} updates failed due to read only column setting", iFail));
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }

        private void OnLocalKeyDownDetect(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                OnLocalPasteClick(sender, e);
            }
            if (e.Control && e.KeyCode == Keys.Insert)
            {
                int iRow = 0;
                if (this.dcmDataGridView_local.CurrentCell != null)
                {
                    iRow = this.dcmDataGridView_local.CurrentCell.RowIndex;
                    if (iRow < 0)
                        iRow = 0;
                    if (iRow < this.dcmDataGridView_local.RowCount)
                        iRow = this.dcmDataGridView_local.RowCount;
                }
                else
                {
                    iRow = this.dcmDataGridView_local.RowCount;
                }
                this.dcmDataGridView_local.Rows.Insert(iRow, 1);
            }
        }


        private void dataGridView_local_CustomContextMenuDetect(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if(dcmDataGridView_local.CurrentRow!=null)
                {
                    Point plocation=dcmDataGridView_local.PointToScreen(dcmDataGridView_local.Location);
                    int eY = MousePosition.Y - plocation.Y;
                    if (eY > this.dcmDataGridView_local.ColumnHeadersHeight)
                    {
                        cellContextMenu_local.Show(dcmDataGridView_local, new Point(MousePosition.X - plocation.X, MousePosition.Y - plocation.Y));
                    }
                }
            }
        }

        private void dataGridView_local_columns_StateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.Column != null && e.StateChanged.Equals(DataGridViewElementStates.Visible))
            {
                CustomColDefGetter.updateCustomColDef(e.Column.Name, e.Column.Visible);
            }
        }

        /// <summary>
        /// 检查同一目录下是否存在已经运行的进程实例，如果存在执行退出操作
        /// </summary>
        public void checkWorkSpace()
        {
            log.Debug("checkWorkSpace(...)");
            if (!Directory.Exists(SysConstants.initEnvDir + SysConstants.cacheDir))
            {
                Directory.CreateDirectory(SysConstants.initEnvDir +SysConstants.cacheDir);
            }
            if (ProcessUtil.checkDuplicateProcess() != null)
            {
#if DEBUG
#else
                MessageBox.Show("当前工作空间下工作进程已存在，确认退出当前实例。", "Notice", MessageBoxButtons.OK);
                Application.Exit();
#endif
            }
        }

        private void startLocalDataRender()
        {
            log.Debug("startDataRender(...)");
            DataExportNoter.loadHistorySIds(SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.exit_note);
            string lastDump = SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.exit_note;
            if (File.Exists(lastDump))
            {
                string lastdumpPath = FileUtil.readAsUTF8Text(lastDump).Trim();
                if (lastdumpPath.Length > 0 && File.Exists(lastdumpPath))
                {
                    localServManager.importData(lastdumpPath);
                }
            }
        }
        public void startGCMSiteRender()
        {
            log.Debug("startGCMSiteRender(...)");
            string name = ConfigurationManager.AppSettings.Get(SysConstants.LUID);
            if (name != null && name.Length > 0)
            {
                gtcache.UserName = name;
                string pwd = ConfigurationManager.AppSettings.Get(SysConstants.LPWD);
                if (pwd != null && pwd.Length > 0)
                {
                    gtcache.Password = Base64DESEncrypt.CreateInstance(name).Decrypt(pwd);
                    gtcache.RememberLogin = true;
                }
            }
        }
        private void OnSimpleMsgTip(object msgTag, params object[] vals)
        {
            ControlAsyncUtil.SyncInvoke(this, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                new IDCM.Forms.MessageDlg(msgTag.ToString()).Show();
            }));
        }
        private void OnGCMItemDetailRender(object msgTag, params object[] vals)
        {
            ControlAsyncUtil.SyncInvoke(dcmDataGridView_gcm, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                int index = 0;
                if(vals!=null && vals.Length>0 && vals[0].GetType().Equals(typeof(int)))
                    index= (int)vals[0];
                gcmServManager.showGCMDataDetail(index);
            }));
        }
        private void OnGCMUserSigned(object msgTag, params object[] vals)
        {
            if (gcmServManager.Signed)
            {
                showGCMDataDlg();
                if (gcmServManager.UserName != null && gcmServManager.UserName.Length>0)
                {
                    ConfigurationHelper.SetAppConfig(SysConstants.LUID, gcmServManager.UserName, SysConstants.defaultCfgPath);
                    if (gtcache.RememberLogin && gcmServManager.Password != null)
                    {
                        ConfigurationHelper.SetAppConfig(SysConstants.LPWD, Base64DESEncrypt.CreateInstance(gcmServManager.UserName).Encrypt(gcmServManager.Password), SysConstants.defaultCfgPath);
                    }
                    else
                    {
                        ConfigurationHelper.SetAppConfig(SysConstants.LPWD,"", SysConstants.defaultCfgPath);
                    }
                }
            }
            else
            {
                showLoginDlg();
            }
        }
        private void OnLocalDataExported(object msgTag, params object[] vals)
        {
            MsgDriver.DCMPublisher.noteSimpleMsg("本地数据导出完成");
        }
        private void OnLocalDataImported(object msgTag, params object[] vals)
        {
            MsgDriver.DCMPublisher.noteSimpleMsg("本地数据导入完成");
        }


        /// <summary>
        /// 单元格的值改变后，执行更新或插入操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_local_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //////////////////////////////////////////////////////////////////
            //if (e.ColumnIndex > 0 && e.RowIndex>0 && this.IsInited)
            //{
            //    DataGridViewRow dgvr = dcmDataGridView_local.Rows[e.RowIndex];
            //    DataGridViewCell statusCell = dgvr.Cells[SysConstants.dgvc_status];
            //    if (statusCell != null)
            //    {
            //        statusCell.Value = "";
            //    }
            //}
            //暂不考虑已导出和未导出的问题
            //////////////////////////////////////////////////////////////////////
        }
        private void dataGridView_gcm_CellClicked(object sender, DataGridViewCellEventArgs e)
        {
            gcmServManager.showGCMDataDetail(e.RowIndex);
        }
        /// <summary>
        /// 拖拽事件运行时的鼠标状态切换方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_local_items_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        /// <summary>
        /// 文件拖拽后事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_local_items_DragDrop(object sender, DragEventArgs e)
        {
            String[] recvs = (String[])e.Data.GetData(DataFormats.FileDrop, false);
            e.Effect = DragDropEffects.None;
            for (int i = 0; i < recvs.Length; i++)
            {
                if (recvs[i].Trim() != "")
                {
                    String fpath = recvs[i].Trim();
                    bool exists = System.IO.File.Exists(fpath);
                    if (exists == true)
                    {
                        localServManager.importData(fpath);
                    }
                }
            }
        }

        /// <summary>
        /// 导入符合特定的文档格式的表单内容
        /// </summary>
        public void openImportDocument()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel文件,mdi缓存文件,DI打包文件(*.xls,*.xlsx,*.xml,*.tsv,*csv,*.json)|*.xls;*.xlsx;*.xml;*.tsv;*.csv;*.json";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fpath = ofd.FileName;
                localServManager.importData(fpath);
            }
        }
        public void exportData()
        {
            localServManager.exportData(this.dcmDataGridView_local);
        }
        public void checkData()
        {
            localServManager.checkData(this.dcmDataGridView_local);
        }
        public string doExitDump()
        {
            if (!RunningHandlerNoter.checkForIdle())
            {
                if (MessageBox.Show("仍有后台任务正在执行中，是否强制退出？", "确认信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    return null;
                }
            }
            this.Enabled = false;
            return localServManager.doExitDump();
        }
        private void gcmTabControl_GCM_SelectedIndexChanging(object sender, DCMControlLib.GCM.SelectedIndexChangingEventArgs e)
        {
            if (e.TabPageIndex == 1)
            {
                if (gcmServManager.Signed)
                {
                    showGCMDataDlg();
                }
                else
                {
                    showLoginDlg();
                }
            }
        }

        private void showLoginDlg()
        {
            ControlAsyncUtil.SyncInvoke(splitContainer_GCM, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                splitContainer_GCM.Panel1Collapsed = false;
                splitContainer_GCM.Panel2Collapsed = true;
                if (textBox_ccinfoId.Text.Length < 1)
                    textBox_ccinfoId.Focus();
                else
                    textBox_pwd.Focus();
            }));
        }

        private void showGCMDataDlg()
        {
            ControlAsyncUtil.SyncInvoke(splitContainer_GCM, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                gcmServManager.refreshGCMDataset();
                splitContainer_GCM.Panel1Collapsed = true;
                splitContainer_GCM.Panel2Collapsed = false;
            }));
        }


        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Select(true, false);
        }

        private void pictureBox_Signhelp_Click(object sender, EventArgs e)
        {
            HelpDocRequester.requestHelpDoc(HelpDocConstants.StartViewTag);
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (this.textBox_pwd.Text.Length <1 || this.textBox_ccinfoId.Text.Length < 1)
            {
                MessageBox.Show("The 'CCInfo Id' and 'GCM Password' should not be empty.");
                return;
            }
            try
            {
                this.panel_GCM_start.Enabled = false;
                gcmServManager.connnectGCM();
            }
            finally
            {
                this.panel_GCM_start.Enabled = true;
            }
        }

        public AsyncServInvoker ServInvoker
        {
            get
            {
                return servInvoker;
            }
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private ContextMenu cellContextMenu_local = null;
        private AsyncServInvoker servInvoker = null;
        private CTableCache ctcache = null;
        private GCMTableCache gtcache = null;
        private volatile bool IsInited = false;
        private GCMServManager gcmServManager = null;
        private LocalServManager localServManager = null;
        private ABCServManager abcServManager = null;
    }
}
