﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDCM.VModule.GCM;
using System.IO;
using IDCM.Base;
using IDCM.Base.Utils;
using IDCM.DataTransfer;
using IDCM.Dlgs;
using System.Configuration;

namespace IDCM
{
    /// <summary>
    /// 包含GCMProView空间的Form窗体，GCMProView控件提供主要的功能入口。
    /// 本实现作为GCMProView的简要集成窗体，作为GCMPro项目的主窗体实现。
    /// 当然不存在后台任务的情形下，关闭本窗体会触发进程退出。
    /// </summary>
    public partial class GCMPro : Form
    {
        #region Constructor&Destructor
        public GCMPro()
        {
            InitializeComponent();
            if (ConfigurationManager.AppSettings[SysConstants.DefaultMaximum].Equals("true", StringComparison.CurrentCultureIgnoreCase))
                this.WindowState = FormWindowState.Maximized;
            this.toolStripStatusLabel_status.Text = GlobalTextRes.Text("Ready");
            this.fileFToolStripMenuItem.Text = GlobalTextRes.Text("File");
            this.openAltOToolStripMenuItem.Text = GlobalTextRes.Text("Open (Alt+O)");
            this.saveAltSToolStripMenuItem.Text = GlobalTextRes.Text("Save (Alt+S)");
            this.quitAltQToolStripMenuItem.Text = GlobalTextRes.Text("Quit(Alt+Q)");
            this.toolStripMenuItem_tool.Text = GlobalTextRes.Text("Tool");
            this.validationAltVToolStripMenuItem.Text = GlobalTextRes.Text("Validation(Alt+V)");
            this.filterAltRToolStripMenuItem.Text = GlobalTextRes.Text("Filter(Alt+R)");
            this.exportAltEToolStripMenuItem.Text = GlobalTextRes.Text("Export(Alt+E)");
            this.searchAltFToolStripMenuItem.Text = GlobalTextRes.Text("Search(Alt+F)");
            this.clearAllAltCToolStripMenuItem.Text = GlobalTextRes.Text("Clear All(Alt+C)");
            this.configurationCToolStripMenuItem.Text = GlobalTextRes.Text("Configuration");
            this.loginGCMToolStripMenuItem.Text = GlobalTextRes.Text("Login GCM(Alt+G)");
            this.languageAltLToolStripMenuItem.Text = GlobalTextRes.Text("Language(Alt+L)");
            this.englishToolStripMenuItem.Text = GlobalTextRes.Text("English");
            this.simplifiedChineseToolStripMenuItem.Text = GlobalTextRes.Text("Simplified Chinese");
            this.helpHToolStripMenuItem.Text = GlobalTextRes.Text("Help");
            this.webSupportAltHToolStripMenuItem.Text = GlobalTextRes.Text("Web Support(Alt+H)");
            this.aboutGCMLiteAltAToolStripMenuItem.Text = GlobalTextRes.Text("About GCMLite(Alt+A)");
            this.offlineDocumentAltDToolStripMenuItem.Text = GlobalTextRes.Text("Offline Document(Alt+D)");
            this.resetColumnsToolStripMenuItem.Text = GlobalTextRes.Text("Reset Local Columns");
            this.toolStripButton_help.ToolTipText = GlobalTextRes.Text("Help");
            this.toolStripButton_search.ToolTipText = GlobalTextRes.Text("Search");
            this.toolStripTextBox_search.ToolTipText = GlobalTextRes.Text("Quick Search");
            this.toolStripButton_down.ToolTipText = GlobalTextRes.Text("DownLoad from GCM");
            this.toolStripButton_pub.ToolTipText = GlobalTextRes.Text("Publish to GCM");
            this.toolStripButton_compare.ToolTipText = GlobalTextRes.Text("Compare with GCM");
            this.toolStripButton_export.ToolTipText = GlobalTextRes.Text("Export");
            this.toolStripButton_import.ToolTipText = GlobalTextRes.Text("Import");
            this.toolStripButton_del.ToolTipText = GlobalTextRes.Text("Delete");
            this.toolStripButton_add.ToolTipText = GlobalTextRes.Text("Add Row");
            viewMonitor = new Timer();
            viewMonitor.Interval = 500;
            viewMonitor.Tick+=viewMonitor_Tick;
            viewMonitor.Start();
        }
        #endregion

        #region Methods

        /******************************************************************
         * 键盘事件处理方法
         * @auther JiahaiWu 2014-03-17
         ******************************************************************/
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Alt | Keys.O://Alt+O按键处理
                    gcmProView_lite.openImportDocument();
                    break;
                case Keys.Alt | Keys.S:
                    gcmProView_lite.saveLocalData(); break;
                case Keys.Alt | Keys.Q:
                    this.Close();
                    break;
                case Keys.Alt | Keys.V:
                    gcmProView_lite.checkLocalData(); break;
                case Keys.Alt | Keys.R:
                    gcmProView_lite.filterToRecvLocalData(); break;
                case Keys.Alt | Keys.E:
                    gcmProView_lite.exportLocalData(); break;
                case Keys.Alt | Keys.C:
                    gcmProView_lite.clearAllLocalData(); break;
                case Keys.Alt | Keys.F:
                    gcmProView_lite.frontFindData(); break;
                case Keys.Alt | Keys.L:
                    configurationCToolStripMenuItem.ShowDropDown();
                    languageAltLToolStripMenuItem.ShowDropDown();break;
                case Keys.Alt | Keys.G:
                    gcmProView_lite.openLoginPage(); break;
                case Keys.Alt | Keys.H:
                    openWebHelpDocument(); break;
                case Keys.Alt | Keys.A:
                    openAboutUsDlg(); break;
                case Keys.Control | Keys.D1://Ctrl+1按键处理
                    gcmProView_lite.checkLocalData();
                    break;
                case Keys.Control | Keys.F:
                    gcmProView_lite.frontFindData();
                    break;
                case Keys.Control | Keys.N:
                    gcmProView_lite.frontFindNext();
                    break;
                case Keys.Control | Keys.P:
                    gcmProView_lite.frontFindPrev();
                    break;
                case Keys.Control | Keys.S://Ctrl+S按键处理
                    gcmProView_lite.saveLocalData(true);
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        private void openAboutUsDlg()
        {
            AboutDlg aboutDlg = new AboutDlg();
            aboutDlg.ShowDialog();
        }
        #endregion

        #region Events&Handlings
        private void viewMonitor_Tick(object sender, EventArgs e)
        {
            if (gcmProView_lite.Enabled ){
                if (gcmProView_lite.OpConditions.Equals(IDCM.VModule.GCM.GCMProView.OpConditionType.Local_View)||
                    gcmProView_lite.OpConditions.Equals(IDCM.VModule.GCM.GCMProView.OpConditionType.Local_Processing))
                {
                    int localCount = gcmProView_lite.LocalRowCount;
                    if (localCount > 0)
                    {
                        string text = localCount.ToString() +" "+ GlobalTextRes.Text("records in total.");
                        this.toolStripLabel_OfficialNotice.Text = text;
                        return;
                    }
                }
                else if (gcmProView_lite.OpConditions.Equals(IDCM.VModule.GCM.GCMProView.OpConditionType.GCM_View))
                {
                    int gcmCount = gcmProView_lite.GCMRowCount;
                    if (gcmCount >0)
                    {
                        string text = gcmCount.ToString() + " " + GlobalTextRes.Text("records in total.");
                        this.toolStripLabel_OfficialNotice.Text = text;
                        return;
                    }
                }
            }
            this.toolStripLabel_OfficialNotice.Text = "";
        }
        /// <summary>
        /// 初始界面加载后事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GCMPro_Load(object sender, EventArgs e)
        {
            gcmProView_lite.initComponenent();

            this.FormClosing+=GCMPro_FormClosing;
            gcmProView_lite.GCMStatusChanged += ServInvoker_OnBottomSatusChange;
            gcmProView_lite.GCMProgressInvoke += ServInvoker_OnProgressChange;
            gcmProView_lite.GCMOpConditionChanged += GCMLite_GCMOpConditionChanging;

            GCMLite_GCMOpConditionChanging(gcmProView_lite.OpConditions);
        }
        /// <summary>
        /// 界面控件状态触发更新事件的处理方法
        /// </summary>
        /// <param name="opType"></param>
        private void GCMLite_GCMOpConditionChanging(GCMProView.OpConditionType opType)
        {
            switch (opType)
            {
                case GCMProView.OpConditionType.Local_View:
                    this.toolStrip_gcmlite.Enabled = true;
                    this.menuStrip_gcmlite.Enabled = true;
                    this.toolStripButton_add.Enabled = true;
                    this.toolStripButton_del.Enabled = true;
                    this.toolStripButton_import.Enabled = true;
                    this.toolStripButton_export.Enabled = true;
                    this.toolStripButton_pub.Enabled = true;
                    this.toolStripButton_compare.Enabled = true;
                    this.toolStripButton_down.Enabled = false;
                    this.toolStripButton_down.Visible = false;
                    openAltOToolStripMenuItem.Enabled = true;
                    saveAltSToolStripMenuItem.Enabled = true;
                    quitAltQToolStripMenuItem.Enabled = true;
                    validationAltVToolStripMenuItem.Enabled = true;
                    filterAltRToolStripMenuItem.Enabled = true;
                    exportAltEToolStripMenuItem.Enabled = true;
                    searchAltFToolStripMenuItem.Enabled = true;
                    clearAllAltCToolStripMenuItem.Enabled = true;
                    resetColumnsToolStripMenuItem.Enabled = true;
                    break;
                case GCMProView.OpConditionType.Local_Processing:
                    this.toolStrip_gcmlite.Enabled = true;
                    this.menuStrip_gcmlite.Enabled = true;
                    this.toolStripButton_add.Enabled = true;
                    this.toolStripButton_del.Enabled = true;
                    this.toolStripButton_import.Enabled = true;
                    this.toolStripButton_export.Enabled = true;
                    this.toolStripButton_compare.Enabled = true;
                    this.toolStripButton_pub.Enabled = true;
                    this.toolStripButton_down.Enabled = false;
                    this.toolStripButton_down.Visible = false;
                    openAltOToolStripMenuItem.Enabled = false;
                    saveAltSToolStripMenuItem.Enabled = false;
                    quitAltQToolStripMenuItem.Enabled = false;
                    validationAltVToolStripMenuItem.Enabled = true;
                    filterAltRToolStripMenuItem.Enabled = true;
                    exportAltEToolStripMenuItem.Enabled = true;
                    searchAltFToolStripMenuItem.Enabled = true;
                    clearAllAltCToolStripMenuItem.Enabled = false;
                    resetColumnsToolStripMenuItem.Enabled = false;
                    break;
                case GCMProView.OpConditionType.GCM_Login:
                    this.toolStrip_gcmlite.Enabled = false;
                    this.menuStrip_gcmlite.Enabled = true;
                    openAltOToolStripMenuItem.Enabled = true;
                    saveAltSToolStripMenuItem.Enabled = true;
                    quitAltQToolStripMenuItem.Enabled = true;
                    validationAltVToolStripMenuItem.Enabled = false;
                    filterAltRToolStripMenuItem.Enabled = false;
                    exportAltEToolStripMenuItem.Enabled = false;
                    searchAltFToolStripMenuItem.Enabled = false;
                    clearAllAltCToolStripMenuItem.Enabled = false;
                    resetColumnsToolStripMenuItem.Enabled = false;
                    break;
                case GCMProView.OpConditionType.GCM_View:
                    this.toolStrip_gcmlite.Enabled = true;
                    this.menuStrip_gcmlite.Enabled = true;
                    this.toolStripButton_add.Enabled = false;
                    this.toolStripButton_del.Enabled = false;
                    this.toolStripButton_import.Enabled = false;
                    this.toolStripButton_export.Enabled = false;
                    this.toolStripButton_pub.Enabled = false;
                    this.toolStripButton_down.Enabled = true;
                    this.toolStripButton_down.Visible = true;
                    openAltOToolStripMenuItem.Enabled = false;
                    saveAltSToolStripMenuItem.Enabled = false;
                    quitAltQToolStripMenuItem.Enabled = true;
                    validationAltVToolStripMenuItem.Enabled = false;
                    filterAltRToolStripMenuItem.Enabled = false;
                    exportAltEToolStripMenuItem.Enabled = false;
                    searchAltFToolStripMenuItem.Enabled = true;
                    clearAllAltCToolStripMenuItem.Enabled = false;
                    resetColumnsToolStripMenuItem.Enabled = false;
                    break;
                case GCMProView.OpConditionType.ABC_View:
                    this.toolStrip_gcmlite.Enabled = true;
                    this.menuStrip_gcmlite.Enabled = true;
                    this.toolStripButton_add.Enabled = false;
                    this.toolStripButton_del.Enabled = false;
                    this.toolStripButton_import.Enabled = false;
                    this.toolStripButton_export.Enabled = false;
                    this.toolStripButton_pub.Enabled = false;
                    this.toolStripButton_down.Enabled = false;
                    openAltOToolStripMenuItem.Enabled = false;
                    saveAltSToolStripMenuItem.Enabled = false;
                    quitAltQToolStripMenuItem.Enabled = true;
                    validationAltVToolStripMenuItem.Enabled = false;
                    filterAltRToolStripMenuItem.Enabled = false;
                    exportAltEToolStripMenuItem.Enabled = false;
                    searchAltFToolStripMenuItem.Enabled = false;
                    clearAllAltCToolStripMenuItem.Enabled = false;
                    resetColumnsToolStripMenuItem.Enabled = false;
                    break;
                case GCMProView.OpConditionType.UnKnown:
                    this.toolStrip_gcmlite.Enabled = false;
                    this.menuStrip_gcmlite.Enabled = false;
                    break;
            }
        }

        private void ServInvoker_OnProgressChange(bool msgTag)
        {
            this.toolStripProgressBar_progress.Visible = msgTag;
        }

        private void ServInvoker_OnBottomSatusChange(string msgTag)
        {
            this.toolStripStatusLabel_status.Text = msgTag;
        }

        private void GCMPro_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!File.Exists(SysConstants.initEnvDir + SysConstants.cacheDir))
                {
                    Directory.CreateDirectory(SysConstants.initEnvDir + SysConstants.cacheDir);
                }
                string dumppath = gcmProView_lite.doDumpWork();
                if (dumppath == null)
                    e.Cancel = true;
                else
                FileUtil.writeToUTF8File(SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.exit_note, dumppath == null ? "" : dumppath);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Exit operation execute failed") + "！", ex);
            }
        }
        

        private void toolStripButton_add_Click(object sender, EventArgs e)
        {
            gcmProView_lite.addLocalDataRow();
        }

        private void toolStripButton_del_Click(object sender, EventArgs e)
        {
            gcmProView_lite.delLocalDataRow();
        }

        private void toolStripButton_import_Click(object sender, EventArgs e)
        {
            gcmProView_lite.openImportDocument();
        }

        private void toolStripButton_export_Click(object sender, EventArgs e)
        {
            gcmProView_lite.exportLocalData();
        }

        private void toolStripButton_pub_Click(object sender, EventArgs e)
        {
            gcmProView_lite.publishLocalData();
        }

        private void toolStripButton_down_Click(object sender, EventArgs e)
        {
            gcmProView_lite.pullGCMData();
        }

        private void toolStripButton_search_Click(object sender, EventArgs e)
        {

            string findTerm = this.toolStripTextBox_search.Text.Trim();
            if(this.toolStripTextBox_search.ForeColor.Equals(Color.DarkGray))
            {
                this.toolStripTextBox_search.Text="";
                this.toolStripTextBox_search.Focus();
            }else if (findTerm.Length > 0)
            {
                gcmProView_lite.quickFindData(findTerm);
            }
        }

        private void toolStripTextBox_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string findTerm = this.toolStripTextBox_search.Text.Trim();
                if (findTerm.Length > 0)
                {
                    gcmProView_lite.quickFindData(findTerm);
                }
            }
        }
        private void toolStripButton_help_Click(object sender, EventArgs e)
        {
            openWebHelpDocument();
        }

        private void openWebHelpDocument()
        {
            gcmProView_lite.requestHelpDoc();
        }

        private void openAltOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.openImportDocument();
        }

        private void saveAltSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.saveLocalData(false);
        }

        private void quitAltQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void validationAltVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.checkLocalData();
        }

        private void filterAltRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.filterToRecvLocalData();
        }

        private void exportAltEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.exportLocalData();
        }

        private void searchAltFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.frontFindData();
        }

        private void clearAllAltCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.clearAllLocalData();
        }

        private void webSupportAltHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openWebHelpDocument();
        }

        private void offlineDocumentAltDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.requestHelpDoc("file:///"+Path.GetDirectoryName(SysConstants.exePath)+"/GCMLite_Help.htm");
        }
        private void aboutGCMLiteAltAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openAboutUsDlg();
        }
        private void loginGCMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.openLoginPage();
        }
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDCM.Base.Utils.ConfigurationHelper.SetAppConfig(SysConstants.CultureInfo, "en-US");
            Application.Restart();
        }

        private void simplifiedChineseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDCM.Base.Utils.ConfigurationHelper.SetAppConfig(SysConstants.CultureInfo, "zh-CN");
            Application.Restart();
        }
        private void languageAltLToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem tsmi in languageAltLToolStripMenuItem.DropDownItems)
            {
                tsmi.Checked = false;
            }
            if (GlobalTextRes.getLanguageName().StartsWith("zh-CN"))
                simplifiedChineseToolStripMenuItem.Checked = true;
            else
                englishToolStripMenuItem.Checked = true;
        }
        private void toolStripButton_compare_Click(object sender, EventArgs e)
        {
            gcmProView_lite.CompareGCMRecords();
        }

        private void resetColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcmProView_lite.ConfigColumns();
        }

        private void toolStripTextBox_search_Enter(object sender, EventArgs e)
        {
            if (this.toolStripTextBox_search.Text.Length > 0 && this.toolStripTextBox_search.ForeColor.Equals(Color.DarkGray))
            {
                this.toolStripTextBox_search.Text = "";
                this.toolStripTextBox_search.ForeColor = Color.Black;
            }
        }

        private void toolStripTextBox_search_Leave(object sender, EventArgs e)
        {
            if (this.toolStripTextBox_search.Text.Length < 1)
            {
                this.toolStripTextBox_search.Text = GlobalTextRes.Text("Quick_Search");
                this.toolStripTextBox_search.ForeColor = Color.DarkGray;
            }
        }
        #endregion

        #region Members
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// GCMProView Monitor
        /// </summary>
        private System.Windows.Forms.Timer viewMonitor = null;
        #endregion

    }
}
