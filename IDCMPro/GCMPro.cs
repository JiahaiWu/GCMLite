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

namespace IDCM
{
    /// <summary>
    /// 包含GCMProView空间的Form窗体，GCMProView控件提供主要的功能入口。
    /// 本实现作为GCMProView的简要集成窗体，作为GCMPro项目的主窗体实现。
    /// 当然不存在后台任务的情形下，关闭本窗体会触发进程退出。
    /// </summary>
    public partial class GCMPro : Form
    {
        public GCMPro()
        {
            InitializeComponent();
            this.toolStripStatusLabel_status.Text = GlobalTextRes.Text(this.toolStripStatusLabel_status.Text);
        }
        /// <summary>
        /// 检查同一目录下是否存在已经运行的进程实例，如果存在执行退出操作
        /// </summary>
        public void checkWorkSpace()
        {
            log.Debug("checkWorkSpace(...)");
            if (!Directory.Exists(SysConstants.initEnvDir + SysConstants.cacheDir))
            {
                Directory.CreateDirectory(SysConstants.initEnvDir + SysConstants.cacheDir);
            }
            if (ProcessUtil.checkDuplicateProcess() != null)
            {
                MessageBox.Show("当前工作空间下工作进程已存在，确认退出当前实例。", "Notice", MessageBoxButtons.OK);
                Application.Exit();
            }
        }

        private void GCMPro_Load(object sender, EventArgs e)
        {
            checkWorkSpace();
            this.FormClosed += GCMPro_FormClosed;
            gcmProView_lite.GCMStatusChanged += ServInvoker_OnBottomSatusChange;
            gcmProView_lite.GCMProgressInvoke += ServInvoker_OnProgressChange;
            gcmProView_lite.GCMOpConditionChanged += GCMLite_GCMOpConditionChanging;

            GCMLite_GCMOpConditionChanging(gcmProView_lite.OpConditions);
        }
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
                    this.toolStripButton_down.Enabled = false;
                    openAltOToolStripMenuItem.Enabled = true;
                    saveAltSToolStripMenuItem.Enabled = true;
                    quitAltQToolStripMenuItem.Enabled = true;
                    validationAltVToolStripMenuItem.Enabled = true;
                    filterAltRToolStripMenuItem.Enabled = true;
                    exportAltEToolStripMenuItem.Enabled = true;
                    searchAltFToolStripMenuItem.Enabled = true;
                    clearAllAltCToolStripMenuItem.Enabled = true;
                    break;
                case GCMProView.OpConditionType.Local_Processing:
                    this.toolStrip_gcmlite.Enabled = true;
                    this.menuStrip_gcmlite.Enabled = true;
                    this.toolStripButton_add.Enabled = true;
                    this.toolStripButton_del.Enabled = true;
                    this.toolStripButton_import.Enabled = true;
                    this.toolStripButton_export.Enabled = true;
                    this.toolStripButton_pub.Enabled = true;
                    this.toolStripButton_down.Enabled = false;
                    openAltOToolStripMenuItem.Enabled = false;
                    saveAltSToolStripMenuItem.Enabled = false;
                    quitAltQToolStripMenuItem.Enabled = false;
                    validationAltVToolStripMenuItem.Enabled = true;
                    filterAltRToolStripMenuItem.Enabled = true;
                    exportAltEToolStripMenuItem.Enabled = true;
                    searchAltFToolStripMenuItem.Enabled = true;
                    clearAllAltCToolStripMenuItem.Enabled = false;
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
                    openAltOToolStripMenuItem.Enabled = false;
                    saveAltSToolStripMenuItem.Enabled = false;
                    quitAltQToolStripMenuItem.Enabled = true;
                    validationAltVToolStripMenuItem.Enabled = false;
                    filterAltRToolStripMenuItem.Enabled = false;
                    exportAltEToolStripMenuItem.Enabled = false;
                    searchAltFToolStripMenuItem.Enabled = true;
                    clearAllAltCToolStripMenuItem.Enabled = false;
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
                    break;
                case GCMProView.OpConditionType.UnKnown:
                    this.toolStrip_gcmlite.Enabled = false;
                    this.menuStrip_gcmlite.Enabled = false;
                    break;
            }
        }

        private void ServInvoker_OnProgressChange(bool msgTag)
        {
            this.toolStripProgressBar_progress.Visible = (bool)msgTag;
        }

        private void ServInvoker_OnBottomSatusChange(string msgTag)
        {
            this.toolStripStatusLabel_status.Text = msgTag;
        }

        private void GCMPro_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (!File.Exists(SysConstants.initEnvDir + SysConstants.cacheDir))
                {
                    Directory.CreateDirectory(SysConstants.initEnvDir + SysConstants.cacheDir);
                }
                string dumppath = gcmProView_lite.doExitDump();
                FileUtil.writeToUTF8File(SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.exit_note, dumppath == null ? "" : dumppath);
            }
            catch (Exception ex)
            {
                log.Error("退出操作执行失败！ " ,ex);
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
            gcmProView_lite.frontFindData();
        }

        private void toolStripTextBox_search_Click(object sender, EventArgs e)
        {
            //////////////////
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

        private void aboutGCMLiteAltAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AboutDlg aboutDlg = new AboutDlg();
            //aboutDlg.ShowDialog();
        }
        /******************************************************************
         * 键盘事件处理方法
         * @auther JiahaiWu 2014-03-17
         ******************************************************************/
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Alt|Keys.O://Alt+O按键处理
                    gcmProView_lite.openImportDocument();
                    break;
                case Keys.Alt | Keys.S:
                    gcmProView_lite.saveLocalData();break;
                case Keys.Alt | Keys.Q:
                    this.Close();
                    break;
                case Keys.Alt | Keys.V:
                    gcmProView_lite.checkLocalData();break;
                case Keys.Alt | Keys.R:
                    gcmProView_lite.filterToRecvLocalData();break;
                case Keys.Alt | Keys.E:
                    gcmProView_lite.exportLocalData();break;
                case Keys.Alt | Keys.C:
                    gcmProView_lite.clearAllLocalData();break;
                case Keys.Alt | Keys.F:
                    gcmProView_lite.frontFindData();break;
                case Keys.Alt | Keys.H:
                    openWebHelpDocument();break;
                case Keys.Alt | Keys.A:
                    openAboutUsDlg();break;
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
            throw new NotImplementedException();
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();


    }
}
