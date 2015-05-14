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
using IDCM.Forms;
using IDCM.MsgDriver;

namespace IDCM.VModule.GCM
{
    /// <summary>
    /// GCMPro定制的用户控件，主体包含三个固定的Tab页签(Local DataSet,GCM Publish,ABC Brwoser)。
    /// 
    /// </summary>
    public partial class GCMProView : UserControl
    {
        #region Constructor&Destructor
        public GCMProView()
        {
            InitializeComponent();
            
            this.button_cancel.Text = GlobalTextRes.Text("Cancel");
            this.checkBox_remember.Text = GlobalTextRes.Text("Remember");
            this.button_confirm.Text = GlobalTextRes.Text("Confirm");
            this.label1.Text = GlobalTextRes.Text("GCM Password")+":";
            this.label_user.Text = GlobalTextRes.Text("CCInfo ID")+":";
            this.tabPage_ABC.Text = GlobalTextRes.Text("ABC Browser");
            this.tabPageEx_Local.Text = GlobalTextRes.Text("Local DataSet");
            this.tabPageEx_GCM.Text = GlobalTextRes.Text("GCM Publish");

            this.Load += GCMProView_Load;
            InitializeMsgDriver();
            InitializeGCMPro();
        }
        #endregion

        #region Methods
        private void InitializeMsgDriver()
        {
            log.Debug("InitializeMsgDriver(...)");
            //////////////////////////////////////////////////////
            //初始化消息池
            servInvoker = new AsyncServInvoker();
            IMsgObserver msgObs = DCMPublisher.initDefaultMsgObserver();
            msgObs.bind(servInvoker);
            ////////////////////////////////////////////////////
            //绑定消息事件处理方法
            servInvoker.OnGCMUserSigned += OnGCMUserSigned;
            servInvoker.OnLocalDataExported += OnLocalDataExported;
            servInvoker.OnLocalDataImported += OnLocalDataImported;
            servInvoker.OnSimpleMsgTip+=OnSimpleMsgTip;
            servInvoker.OnGCMItemDetailRender += OnGCMItemDetailRender;
            servInvoker.OnBottomSatusChange += OnBottomSatusChange;
            servInvoker.OnProgressChange += OnProgressChange;
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
                if (ccds != null && ccds.Count > 0)
                {
                    ctcache = new CTableCache(dcmDataGridView_local,ccds);
                    ////////////////////////////////////////////////////
                    //设定本地数据表属性及事件处理方法
                    localServManager = new LocalServManager(ctcache);
                    this.dcmDataGridView_local.AllowDrop = true;
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
                    this.dcmDataGridView_local.ColumnHeaderMouseClick+=dcmDataGridView_local_ColumnHeaderMouseClick;
                    localFrontFindDlg = new LocalFrontFindDlg(dcmDataGridView_local);
                    localFrontFindDlg.setCellHit += new LocalFrontFindDlg.SetHit<DataGridViewCell>(setDGVCellHit);
                    localFrontFindDlg.cancelCellHit += new LocalFrontFindDlg.CancelHit<DataGridViewCell>(cancelDGVCellHit);
                    //加载GCM发布数据表
                    gtcache = new GCMTableCache(textBox_ccinfoId, textBox_pwd, checkBox_remember, dcmDataGridView_gcm, dcmTreeView_gcm);
                    gcmServManager = new GCMServManager(gtcache);
                    this.dcmDataGridView_gcm.CellClick += dataGridView_gcm_CellClicked;
                    this.dcmDataGridView_gcm.AllowUserToResizeColumns = true;
                    this.dcmDataGridView_gcm.AllowUserToResizeRows = true;
                    this.dcmDataGridView_gcm.CellMouseClick+=dcmDataGridView_gcm_CellMouseClick;
                    this.cellContextMenu_gcm = new System.Windows.Forms.ContextMenu();
                    this.cellContextMenu_gcm.MenuItems.Add(new MenuItem("Search record", OnGCMSearchClick));

                    gcmFrontFindDlg = new GCMFrontFindDlg(dcmDataGridView_gcm);
                    gcmFrontFindDlg.setCellHit += new GCMFrontFindDlg.SetHit<DataGridViewCell>(setDGVCellHit);
                    gcmFrontFindDlg.cancelCellHit += new GCMFrontFindDlg.CancelHit<DataGridViewCell>(cancelDGVCellHit);
                    //加载ABC WebKit
                    abcServManager = new ABCServManager(abcBrowser_abc);
                    ////////////////////////////////////////////////////
                    //设置初始化的页签
                    gcmTabControl_GCM.SelectedIndex = tabPageEx_Local.TabIndex;
                    opCond = OpConditionType.Local_View;
                    ////////////////////////////////////////////////////
                    this.IsInited = true;
                }
            }
            catch (IDCMException ex)
            {
                this.IsInited = false;
                log.Error(IDCM.Base.GlobalTextRes.Text("Application View Initialize Failed") + "!", ex);
                MessageBox.Show(IDCM.Base.GlobalTextRes.Text("Application View Initialize Failed") + "! @Message=" + ex.Message + " \n" + ex.ToString());
            }
            finally
            {
                this.Enabled = this.IsInited;
                this.Visible = this.Enabled;
            }
        }
        
        private void setDGVCellHit(DataGridViewCell cell)
        {
            if (cell.Visible == false)
                return;
            cell.DataGridView.EndEdit();
            int colCount = DGVUtil.getTextColumnCount(cell.DataGridView);
            DataGridViewCell rightCell = cell.DataGridView.Rows[cell.RowIndex].Cells[colCount - 1];
            while (rightCell.Visible == false && rightCell.ColumnIndex > -1)
            {
                rightCell = rightCell.OwningRow.Cells[rightCell.ColumnIndex - 1];
            }
            cell.DataGridView.CurrentCell = rightCell;
            cell.DataGridView.CurrentCell = cell;
            cell.Selected = true;
            cell.DataGridView.BeginEdit(true);
        }
        private void cancelDGVCellHit(DataGridViewCell cell)
        {
            if (cell.Visible == false)
                return;
            cell.DataGridView.EndEdit();
            cell.Selected = false;
        }
        
        private void startLocalDataRender()
        {
            log.Debug("startDataRender(...)");
            DataExportNoter.loadHistorySIds(SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.exit_note);
            string lastDump = SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.exit_note;
            if (File.Exists(lastDump))
            {
                string lastdumpPath = new LocalDataDumper().LastDumpPath;
                if (lastdumpPath!=null && lastdumpPath.Length > 0 && File.Exists(lastdumpPath))
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
        public void exportLocalData()
        {
            localServManager.exportData(this.dcmDataGridView_local);
        }
        public void checkLocalData()
        {
            localServManager.checkLocalData();
        }
        public string doExitDump()
        {
            CustomColDefGetter.saveUpdatedHistCfg();
            if (!RunningHandlerNoter.checkForIdle())
            {
                if (MessageBox.Show(GlobalTextRes.Text("There are background tasks are executing, force quit or not"),
                    GlobalTextRes.Text("Confirm Message"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    return null;
                }
            }
            return localServManager.doExitDump();
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
            notifyOpConditions(OpConditionType.GCM_Login);
        }

        private void showGCMDataDlg()
        {
            ControlAsyncUtil.SyncInvoke(splitContainer_GCM, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                gcmServManager.refreshGCMDataset();
                splitContainer_GCM.Panel1Collapsed = true;
                splitContainer_GCM.Panel2Collapsed = false;
            }));
            notifyOpConditions(OpConditionType.GCM_View);
        }
        

        public void addLocalDataRow()
        {
            dcmDataGridView_local.Rows.Add();
        }

        public void delLocalDataRow()
        {
            if (dcmDataGridView_local.SelectedRows != null)
            {
                foreach(DataGridViewRow dgvr in dcmDataGridView_local.SelectedRows)
                {
                    dcmDataGridView_local.Rows.Remove(dgvr);
                }
                
            }else if (dcmDataGridView_local.CurrentRow != null)
            {
                dcmDataGridView_local.Rows.RemoveAt(dcmDataGridView_local.CurrentRow.Index);
            }
        }

        public void publishLocalData()
        {
            DataGridViewSelectedRowCollection selectedRows = dcmDataGridView_local.SelectedRows;
            if (selectedRows != null && selectedRows.Count > 0)
            {
                if (gcmServManager.Signed)
                {
                    localServManager.publishLocalDataToGCM(gcmServManager.getAuthInfo(), selectedRows);
                }
                else
                {
                    MessageBox.Show(GlobalTextRes.Text("Please Login before submitting to GCM."));
                    this.gcmTabControl_GCM.SelectedIndex = tabPageEx_GCM.TabIndex;
                }
            }
            else if (dcmDataGridView_local.CurrentCell != null)
            {
                if (gcmServManager.Signed)
                {
                    localServManager.publishLocalDataToGCM(gcmServManager.getAuthInfo(), dcmDataGridView_local.CurrentRow);
                }
                else
                {
                    MessageBox.Show(GlobalTextRes.Text("Please Login before submitting to GCM."));
                    this.gcmTabControl_GCM.SelectedIndex = tabPageEx_GCM.TabIndex;
                }
            }
        }

        public void pullGCMData()
        {
            gcmServManager.downGCMData(dcmDataGridView_gcm);
        }
        public void requestHelpDoc()
        {
            HelpDocRequester.requestHelpDoc();
        }
        public void frontFindData()
        {
            if (opCond.Equals(OpConditionType.Local_View) || opCond.Equals(OpConditionType.Local_Processing))
            {
                localFrontFindDlg.BringToFront();
                localFrontFindDlg.Visible = true;
                localFrontFindDlg.Show();
            }
            else if (opCond.Equals(OpConditionType.GCM_View))
            {
                gcmFrontFindDlg.BringToFront();
                gcmFrontFindDlg.Visible = true;
                gcmFrontFindDlg.Show();
            }
        }
        public void frontFindNext()
        {
            if (opCond.Equals(OpConditionType.Local_View) || opCond.Equals(OpConditionType.Local_Processing))
            {
                localFrontFindDlg.findDown();
            }
            else if (opCond.Equals(OpConditionType.GCM_View))
            {
                gcmFrontFindDlg.findDown();
            }
        }
        public void frontFindPrev()
        {
            if (opCond.Equals(OpConditionType.Local_View) || opCond.Equals(OpConditionType.Local_Processing))
            {
                localFrontFindDlg.findRev();
            }
            else if (opCond.Equals(OpConditionType.GCM_View))
            {
                gcmFrontFindDlg.findRev();
            }
        }

        public void filterToRecvLocalData()
        {
            localServManager.filterToRecvLocalData();
        }

        public void clearAllLocalData()
        {
            if(opCond.Equals(OpConditionType.Local_View))
            {
                this.dcmDataGridView_local.Rows.Clear();
            }
        }

        public void saveLocalData(bool useDefaultPath = true)
        {
            try
            {
                if (!File.Exists(SysConstants.initEnvDir + SysConstants.cacheDir))
                {
                    Directory.CreateDirectory(SysConstants.initEnvDir + SysConstants.cacheDir);
                }
                string dumppath = doExitDump();
                FileUtil.writeToUTF8File(SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.exit_note, dumppath == null ? "" : dumppath);
            }
            catch (Exception ex)
            {
                log.Error(GlobalTextRes.Text("Exit operation execute failed")+"！ ", ex);
            }
        }
        private void notifyOpConditions(OpConditionType opType)
        {
            if (opCond != opType)
            {
                opCond = opType;
                ControlAsyncUtil.SyncInvoke(this, new ControlAsyncUtil.InvokeHandler(delegate()
                {
                    if (GCMOpConditionChanged != null)
                        GCMOpConditionChanged(OpConditions);
                }));
            }
        }
        #endregion

        #region Events&Handlings

        public event GCMStatusHandler GCMStatusChanged;
        public event GCMProgressHandler GCMProgressInvoke;
        public event GCMOpConditionHandler GCMOpConditionChanged;

        private void GCMProView_Load(object sender, EventArgs e)
        {

            startLocalDataRender();
            startGCMSiteRender();
        }
        private void colConfiger_ColConfigChanged(int cursor, bool isRequire, bool isUnique, string restrict)
        {
            if (cursor > -1)
            {
                ControlAsyncUtil.SyncInvoke(dcmDataGridView_local, new ControlAsyncUtil.InvokeHandler(delegate()
                {
                    DataGridViewColumn dgvc = dcmDataGridView_local.Columns[cursor];
                    if (dgvc != null && dgvc.Visible)
                    {
                        CustomColDefGetter.updateCustomColCond(dgvc.Name, isRequire, isUnique, restrict);
                        MsgDriver.DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Column restrictions updated."));
                    }
                }));
            }
        }

        private void dcmDataGridView_local_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control)
            {
                if (e.ColumnIndex > -1)
                {
                    CustomColDef ccd = CustomColDefGetter.getCustomColDef(dcmDataGridView_local.Columns[e.ColumnIndex].Name);
                    if (ccd != null && ccd.IsEnable)
                    {
                        ColConfigDlg colConfiger = new ColConfigDlg(e.ColumnIndex, ccd.Alias, ccd.IsRequire, ccd.IsUnique, ccd.Restrict);
                        colConfiger.ColConfigChanged += colConfiger_ColConfigChanged;
                        colConfiger.Show(this.FindForm(), new Point(MousePosition.X, MousePosition.Y));
                    }
                }
            }
        }
        private void OnLocalSubmitClick(object sender, EventArgs e)
        {
            publishLocalData();
        }
        private void OnLocalSearchClick(object sender, EventArgs e)
        {
            if (dcmDataGridView_local.CurrentRow != null)
            {
                DataGridViewCellCollection dgvcc = dcmDataGridView_local.CurrentRow.Cells;
                string strainId = dgvcc[ctcache.getKeyColIndex()].FormattedValue.ToString();
                if (abcServManager.linkTo(strainId))
                    gcmTabControl_GCM.SelectedIndex = tabPage_ABC.TabIndex;
            }
        }
        private void OnGCMSearchClick(object sender, EventArgs e)
        {
            if (dcmDataGridView_gcm.CurrentRow != null)
            {
                DataGridViewCellCollection dgvcc = dcmDataGridView_gcm.CurrentRow.Cells;
                if (gtcache.getStrainKeyColIndex() > -1)
                {
                    string strainId = dgvcc[gtcache.getStrainKeyColIndex()].FormattedValue.ToString();
                    if (abcServManager.linkTo(strainId))
                        gcmTabControl_GCM.SelectedIndex = tabPage_ABC.TabIndex;
                }
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
                    /////////////////////////////////////////////////////////////////////////////
                    //if (iFail > 0)
                    //    MessageBox.Show(string.Format("{0} updates failed due to read only column setting", iFail));
                    //@Deprecated
                    /////////////////////////////////////////////////////////////////////////////
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(GlobalTextRes.Text("The data you pasted is in the wrong format for the cell"));
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
                if (dcmDataGridView_local.CurrentRow != null)
                {
                    Point plocation = dcmDataGridView_local.PointToScreen(dcmDataGridView_local.Location);
                    int eY = MousePosition.Y - plocation.Y;
                    if (eY > this.dcmDataGridView_local.ColumnHeadersHeight)
                    {
                        cellContextMenu_local.Show(dcmDataGridView_local, new Point(MousePosition.X - plocation.X, MousePosition.Y - plocation.Y));
                    }
                }
            }
        }

        private void dcmDataGridView_gcm_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dcmDataGridView_gcm.CurrentRow != null)
                {
                    Point plocation = dcmDataGridView_gcm.PointToScreen(dcmDataGridView_gcm.Location);
                    int eY = MousePosition.Y - plocation.Y;
                    if (eY > this.dcmDataGridView_gcm.ColumnHeadersHeight)
                    {
                        cellContextMenu_gcm.Show(dcmDataGridView_gcm, new Point(MousePosition.X - plocation.X, MousePosition.Y - plocation.Y));
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
                if (vals != null && vals.Length > 0 && vals[0].GetType().Equals(typeof(int)))
                    index = (int)vals[0];
                gcmServManager.showGCMDataDetail(index);
            }));
        }
        private void OnGCMUserSigned(object msgTag, params object[] vals)
        {
            if (gcmServManager.Signed)
            {
                showGCMDataDlg();
                if (gcmServManager.UserName != null && gcmServManager.UserName.Length > 0)
                {
                    ConfigurationHelper.SetAppConfig(SysConstants.LUID, gcmServManager.UserName, SysConstants.defaultCfgPath);
                    if (gtcache.RememberLogin && gcmServManager.Password != null)
                    {
                        ConfigurationHelper.SetAppConfig(SysConstants.LPWD, Base64DESEncrypt.CreateInstance(gcmServManager.UserName).Encrypt(gcmServManager.Password), SysConstants.defaultCfgPath);
                    }
                    else
                    {
                        ConfigurationHelper.SetAppConfig(SysConstants.LPWD, "", SysConstants.defaultCfgPath);
                    }
                }
            }
            else
            {
                showLoginDlg();
            }
        }
        private void OnBottomSatusChange(object msgTag, params object[] vals)
        {
            ControlAsyncUtil.SyncInvoke(this, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (GCMStatusChanged != null)
                {
                    GCMStatusChanged((msgTag != null) ? msgTag.ToString() : GlobalTextRes.Text("Ready"));
                }
            }));
        }
        private void OnProgressChange(object msgTag, params object[] vals)
        {
            ControlAsyncUtil.SyncInvoke(this, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (GCMProgressInvoke != null)
                {
                    if (msgTag.GetType().Equals(typeof(bool)))
                    {
                        GCMProgressInvoke((bool)msgTag);
                    }
                }
            }));
        }

        private void gcmTabControl_GCM_SelectedIndexChanging(object sender, DCMControlLib.GCM.SelectedIndexChangingEventArgs e)
        {
            if (e.TabPageIndex == tabPageEx_GCM.TabIndex)
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
            else if (e.TabPageIndex == tabPage_ABC.TabIndex)
                notifyOpConditions(OpConditionType.ABC_View);
            else
                notifyOpConditions(OpConditionType.Local_View);
        }


        private void OnLocalDataExported(object msgTag, params object[] vals)
        {
            MsgDriver.DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Local data exported success"));
        }
        private void OnLocalDataImported(object msgTag, params object[] vals)
        {
            MsgDriver.DCMPublisher.noteSimpleMsg(IDCM.Base.GlobalTextRes.Text("Local data import success"));
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
            if (this.textBox_pwd.Text.Length < 1 || this.textBox_ccinfoId.Text.Length < 1)
            {
                MessageBox.Show(GlobalTextRes.Text("The 'CCInfo Id' and 'GCM Password' should not be empty."));
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

        private void splitContainer_GCM_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Point reNewPoint = new Point(this.splitContainer_GCM.Panel1.Width / 2 - this.panel_GCM_start.Width / 2, (int)(this.splitContainer_GCM.Panel1.Height * 0.82 - this.panel_GCM_start.Height));
            this.panel_GCM_start.Location = reNewPoint;
        }
        #endregion

        #region Property
        public OpConditionType OpConditions
        {
            get
            {
                if (opCond.Equals(OpConditionType.Local_View)||opCond.Equals(OpConditionType.Local_Processing))
                    return RunningHandlerNoter.checkForIdle() ? opCond : OpConditionType.Local_Processing;
                else
                    return opCond;
            }
        }
        #endregion

        #region Members
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private ContextMenu cellContextMenu_local = null;
        private ContextMenu cellContextMenu_gcm = null;
        private LocalFrontFindDlg localFrontFindDlg = null;
        private GCMFrontFindDlg gcmFrontFindDlg = null;
        private AsyncServInvoker servInvoker = null;
        private CTableCache ctcache = null;
        private GCMTableCache gtcache = null;
        private volatile bool IsInited = false;
        private GCMServManager gcmServManager = null;
        private LocalServManager localServManager = null;
        private ABCServManager abcServManager = null;
        private OpConditionType opCond = OpConditionType.UnKnown;
        
        //异步消息事件委托形式化声明
        public delegate void GCMOpConditionHandler(OpConditionType opType);
        public delegate void GCMProgressHandler(bool running);
        public delegate void GCMStatusHandler(string status);
        #endregion

        #region Constants
        public enum OpConditionType
        {
            Local_View=0,
            Local_Processing=1,
            GCM_Login=2,
            GCM_View=3,
            ABC_View=4,
            UnKnown=5
        }
        #endregion
    }
}
