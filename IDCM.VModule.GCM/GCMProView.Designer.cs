namespace IDCM.VModule.GCM
{
    partial class GCMProView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GCMProView));
            this.gcmTabControl_GCM = new DCMControlLib.GCM.GCMTabControl();
            this.tabPageEx_Local = new DCMControlLib.GCM.TabPageEx();
            this.splitContainer_local = new System.Windows.Forms.SplitContainer();
            this.dcmDataGridView_local = new DCMControlLib.DCMDataGridView();
            this.tabPageEx_GCM = new DCMControlLib.GCM.TabPageEx();
            this.splitContainer_GCM = new System.Windows.Forms.SplitContainer();
            this.panel_GCM_start = new System.Windows.Forms.Panel();
            this.pictureBox_Signhelp = new System.Windows.Forms.PictureBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.checkBox_remember = new System.Windows.Forms.CheckBox();
            this.button_confirm = new System.Windows.Forms.Button();
            this.textBox_pwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ccinfoId = new System.Windows.Forms.TextBox();
            this.label_user = new System.Windows.Forms.Label();
            this.splitContainer_GCMData = new System.Windows.Forms.SplitContainer();
            this.dcmDataGridView_gcm = new DCMControlLib.DCMDataGridView();
            this.dcmTreeView_gcm = new DCMControlLib.Tree.DCMTreeView();
            this.imageList_gcmtree = new System.Windows.Forms.ImageList(this.components);
            this.tabPage_ABC = new DCMControlLib.GCM.TabPageEx();
            this.splitContainer_abc = new System.Windows.Forms.SplitContainer();
            this.abcBrowser_abc = new DCMControlLib.GCM.ABCBrowser();
            this.imageList_tab = new System.Windows.Forms.ImageList(this.components);
            this.gcmTabControl_GCM.SuspendLayout();
            this.tabPageEx_Local.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_local)).BeginInit();
            this.splitContainer_local.Panel1.SuspendLayout();
            this.splitContainer_local.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dcmDataGridView_local)).BeginInit();
            this.tabPageEx_GCM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_GCM)).BeginInit();
            this.splitContainer_GCM.Panel1.SuspendLayout();
            this.splitContainer_GCM.Panel2.SuspendLayout();
            this.splitContainer_GCM.SuspendLayout();
            this.panel_GCM_start.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Signhelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_GCMData)).BeginInit();
            this.splitContainer_GCMData.Panel1.SuspendLayout();
            this.splitContainer_GCMData.Panel2.SuspendLayout();
            this.splitContainer_GCMData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dcmDataGridView_gcm)).BeginInit();
            this.tabPage_ABC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_abc)).BeginInit();
            this.splitContainer_abc.Panel1.SuspendLayout();
            this.splitContainer_abc.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcmTabControl_GCM
            // 
            this.gcmTabControl_GCM.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.gcmTabControl_GCM.Alignments = DCMControlLib.GCM.GCMTabControl.TabAlignments.Bottom;
            this.gcmTabControl_GCM.AllowDrop = true;
            this.gcmTabControl_GCM.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gcmTabControl_GCM.BackgroundHatcher.HatchType = System.Drawing.Drawing2D.HatchStyle.DashedVertical;
            this.gcmTabControl_GCM.Controls.Add(this.tabPageEx_Local);
            this.gcmTabControl_GCM.Controls.Add(this.tabPageEx_GCM);
            this.gcmTabControl_GCM.Controls.Add(this.tabPage_ABC);
            this.gcmTabControl_GCM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcmTabControl_GCM.ImageList = this.imageList_tab;
            this.gcmTabControl_GCM.IsCaptionVisible = false;
            this.gcmTabControl_GCM.ItemSize = new System.Drawing.Size(150, 30);
            this.gcmTabControl_GCM.Location = new System.Drawing.Point(0, 0);
            this.gcmTabControl_GCM.Name = "gcmTabControl_GCM";
            this.gcmTabControl_GCM.SelectedIndex = 0;
            this.gcmTabControl_GCM.Size = new System.Drawing.Size(716, 513);
            this.gcmTabControl_GCM.TabGradient.ColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(223)))), ((int)(((byte)(246)))));
            this.gcmTabControl_GCM.TabGradient.ColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(67)))), ((int)(((byte)(164)))));
            this.gcmTabControl_GCM.TabGradient.GradientStyle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gcmTabControl_GCM.TabGradient.TabPageSelectedTextColor = System.Drawing.Color.White;
            this.gcmTabControl_GCM.TabGradient.TabPageTextColor = System.Drawing.Color.DimGray;
            this.gcmTabControl_GCM.TabIndex = 0;
            this.gcmTabControl_GCM.TabStyles = DCMControlLib.GCM.GCMTabControl.TabStyle.VS2010;
            this.gcmTabControl_GCM.UpDownStyle = DCMControlLib.GCM.GCMTabControl.UpDown32Style.Default;
            this.gcmTabControl_GCM.SelectedIndexChanging += new System.EventHandler<DCMControlLib.GCM.SelectedIndexChangingEventArgs>(this.gcmTabControl_GCM_SelectedIndexChanging);
            // 
            // tabPageEx_Local
            // 
            this.tabPageEx_Local.BackColor = System.Drawing.Color.White;
            this.tabPageEx_Local.Controls.Add(this.splitContainer_local);
            this.tabPageEx_Local.Font = new System.Drawing.Font("Arial", 10F);
            this.tabPageEx_Local.ImageIndex = 2;
            this.tabPageEx_Local.IsClosable = false;
            this.tabPageEx_Local.Location = new System.Drawing.Point(1, 5);
            this.tabPageEx_Local.Name = "tabPageEx_Local";
            this.tabPageEx_Local.Size = new System.Drawing.Size(714, 472);
            this.tabPageEx_Local.TabIndex = 0;
            this.tabPageEx_Local.Text = "Local DataSet";
            // 
            // splitContainer_local
            // 
            this.splitContainer_local.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_local.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_local.Name = "splitContainer_local";
            // 
            // splitContainer_local.Panel1
            // 
            this.splitContainer_local.Panel1.Controls.Add(this.dcmDataGridView_local);
            this.splitContainer_local.Panel2Collapsed = true;
            this.splitContainer_local.Size = new System.Drawing.Size(714, 472);
            this.splitContainer_local.SplitterDistance = 492;
            this.splitContainer_local.TabIndex = 0;
            // 
            // dcmDataGridView_local
            // 
            this.dcmDataGridView_local.AllowDrop = true;
            this.dcmDataGridView_local.AllowUserToAddRows = false;
            this.dcmDataGridView_local.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 10F);
            this.dcmDataGridView_local.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dcmDataGridView_local.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dcmDataGridView_local.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmDataGridView_local.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dcmDataGridView_local.EnableHeadersVisualStyles = false;
            this.dcmDataGridView_local.Location = new System.Drawing.Point(0, 0);
            this.dcmDataGridView_local.Margin = new System.Windows.Forms.Padding(0);
            this.dcmDataGridView_local.Name = "dcmDataGridView_local";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 10F);
            this.dcmDataGridView_local.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dcmDataGridView_local.RowTemplate.Height = 23;
            this.dcmDataGridView_local.Size = new System.Drawing.Size(714, 472);
            this.dcmDataGridView_local.TabIndex = 0;
            // 
            // tabPageEx_GCM
            // 
            this.tabPageEx_GCM.BackColor = System.Drawing.Color.White;
            this.tabPageEx_GCM.Controls.Add(this.splitContainer_GCM);
            this.tabPageEx_GCM.Font = new System.Drawing.Font("Arial", 10F);
            this.tabPageEx_GCM.ImageIndex = 1;
            this.tabPageEx_GCM.IsClosable = false;
            this.tabPageEx_GCM.Location = new System.Drawing.Point(1, 5);
            this.tabPageEx_GCM.Name = "tabPageEx_GCM";
            this.tabPageEx_GCM.Size = new System.Drawing.Size(714, 472);
            this.tabPageEx_GCM.TabIndex = 1;
            this.tabPageEx_GCM.Text = "GCM Publish";
            // 
            // splitContainer_GCM
            // 
            this.splitContainer_GCM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_GCM.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_GCM.Name = "splitContainer_GCM";
            // 
            // splitContainer_GCM.Panel1
            // 
            this.splitContainer_GCM.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer_GCM.Panel1.BackgroundImage = global::IDCM.Properties.Resources.initView;
            this.splitContainer_GCM.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.splitContainer_GCM.Panel1.Controls.Add(this.panel_GCM_start);
            // 
            // splitContainer_GCM.Panel2
            // 
            this.splitContainer_GCM.Panel2.Controls.Add(this.splitContainer_GCMData);
            this.splitContainer_GCM.Size = new System.Drawing.Size(714, 472);
            this.splitContainer_GCM.SplitterDistance = 228;
            this.splitContainer_GCM.TabIndex = 0;
            // 
            // panel_GCM_start
            // 
            this.panel_GCM_start.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_GCM_start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(255)))), ((int)(((byte)(212)))));
            this.panel_GCM_start.Controls.Add(this.pictureBox_Signhelp);
            this.panel_GCM_start.Controls.Add(this.button_cancel);
            this.panel_GCM_start.Controls.Add(this.checkBox_remember);
            this.panel_GCM_start.Controls.Add(this.button_confirm);
            this.panel_GCM_start.Controls.Add(this.textBox_pwd);
            this.panel_GCM_start.Controls.Add(this.label1);
            this.panel_GCM_start.Controls.Add(this.textBox_ccinfoId);
            this.panel_GCM_start.Controls.Add(this.label_user);
            this.panel_GCM_start.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel_GCM_start.Location = new System.Drawing.Point(-139, 311);
            this.panel_GCM_start.Margin = new System.Windows.Forms.Padding(0);
            this.panel_GCM_start.Name = "panel_GCM_start";
            this.panel_GCM_start.Size = new System.Drawing.Size(480, 80);
            this.panel_GCM_start.TabIndex = 3;
            // 
            // pictureBox_Signhelp
            // 
            this.pictureBox_Signhelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Signhelp.Image = global::IDCM.Properties.Resources.help;
            this.pictureBox_Signhelp.Location = new System.Drawing.Point(294, 19);
            this.pictureBox_Signhelp.Name = "pictureBox_Signhelp";
            this.pictureBox_Signhelp.Size = new System.Drawing.Size(21, 21);
            this.pictureBox_Signhelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Signhelp.TabIndex = 11;
            this.pictureBox_Signhelp.TabStop = false;
            this.pictureBox_Signhelp.Click += new System.EventHandler(this.pictureBox_Signhelp_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.BackColor = System.Drawing.Color.Transparent;
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_cancel.Location = new System.Drawing.Point(396, 15);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 9;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = false;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // checkBox_remember
            // 
            this.checkBox_remember.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_remember.AutoSize = true;
            this.checkBox_remember.Checked = true;
            this.checkBox_remember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_remember.Location = new System.Drawing.Point(295, 48);
            this.checkBox_remember.Name = "checkBox_remember";
            this.checkBox_remember.Size = new System.Drawing.Size(82, 18);
            this.checkBox_remember.TabIndex = 7;
            this.checkBox_remember.Text = "Remember";
            this.checkBox_remember.UseVisualStyleBackColor = true;
            // 
            // button_confirm
            // 
            this.button_confirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_confirm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_confirm.Location = new System.Drawing.Point(397, 45);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 6;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // textBox_pwd
            // 
            this.textBox_pwd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_pwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_pwd.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBox_pwd.Location = new System.Drawing.Point(112, 46);
            this.textBox_pwd.Name = "textBox_pwd";
            this.textBox_pwd.ShortcutsEnabled = false;
            this.textBox_pwd.Size = new System.Drawing.Size(175, 23);
            this.textBox_pwd.TabIndex = 5;
            this.textBox_pwd.Tag = "";
            this.textBox_pwd.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "GCM Password:";
            // 
            // textBox_ccinfoId
            // 
            this.textBox_ccinfoId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ccinfoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ccinfoId.Location = new System.Drawing.Point(112, 18);
            this.textBox_ccinfoId.Name = "textBox_ccinfoId";
            this.textBox_ccinfoId.Size = new System.Drawing.Size(175, 23);
            this.textBox_ccinfoId.TabIndex = 3;
            // 
            // label_user
            // 
            this.label_user.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_user.AutoSize = true;
            this.label_user.Location = new System.Drawing.Point(31, 22);
            this.label_user.Name = "label_user";
            this.label_user.Size = new System.Drawing.Size(77, 14);
            this.label_user.TabIndex = 2;
            this.label_user.Text = "CCInfo ID:";
            // 
            // splitContainer_GCMData
            // 
            this.splitContainer_GCMData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_GCMData.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_GCMData.Name = "splitContainer_GCMData";
            // 
            // splitContainer_GCMData.Panel1
            // 
            this.splitContainer_GCMData.Panel1.Controls.Add(this.dcmDataGridView_gcm);
            // 
            // splitContainer_GCMData.Panel2
            // 
            this.splitContainer_GCMData.Panel2.Controls.Add(this.dcmTreeView_gcm);
            this.splitContainer_GCMData.Size = new System.Drawing.Size(482, 472);
            this.splitContainer_GCMData.SplitterDistance = 320;
            this.splitContainer_GCMData.TabIndex = 0;
            // 
            // dcmDataGridView_gcm
            // 
            this.dcmDataGridView_gcm.AllowUserToAddRows = false;
            this.dcmDataGridView_gcm.AllowUserToDeleteRows = false;
            this.dcmDataGridView_gcm.AllowUserToOrderColumns = true;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 10F);
            this.dcmDataGridView_gcm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dcmDataGridView_gcm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dcmDataGridView_gcm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmDataGridView_gcm.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dcmDataGridView_gcm.EnableHeadersVisualStyles = false;
            this.dcmDataGridView_gcm.Location = new System.Drawing.Point(0, 0);
            this.dcmDataGridView_gcm.Name = "dcmDataGridView_gcm";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 10F);
            this.dcmDataGridView_gcm.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dcmDataGridView_gcm.RowTemplate.Height = 23;
            this.dcmDataGridView_gcm.Size = new System.Drawing.Size(320, 472);
            this.dcmDataGridView_gcm.TabIndex = 0;
            // 
            // dcmTreeView_gcm
            // 
            this.dcmTreeView_gcm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmTreeView_gcm.ImageIndex = 0;
            this.dcmTreeView_gcm.ImageList = this.imageList_gcmtree;
            this.dcmTreeView_gcm.Location = new System.Drawing.Point(0, 0);
            this.dcmTreeView_gcm.Name = "dcmTreeView_gcm";
            this.dcmTreeView_gcm.SelectedImageIndex = 0;
            this.dcmTreeView_gcm.Size = new System.Drawing.Size(158, 472);
            this.dcmTreeView_gcm.TabIndex = 0;
            // 
            // imageList_gcmtree
            // 
            this.imageList_gcmtree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_gcmtree.ImageStream")));
            this.imageList_gcmtree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_gcmtree.Images.SetKeyName(0, "folder.gif");
            this.imageList_gcmtree.Images.SetKeyName(1, "document_lined.png");
            // 
            // tabPage_ABC
            // 
            this.tabPage_ABC.BackColor = System.Drawing.Color.White;
            this.tabPage_ABC.Controls.Add(this.splitContainer_abc);
            this.tabPage_ABC.Font = new System.Drawing.Font("Arial", 10F);
            this.tabPage_ABC.ImageIndex = 0;
            this.tabPage_ABC.IsClosable = false;
            this.tabPage_ABC.Location = new System.Drawing.Point(1, 5);
            this.tabPage_ABC.Name = "tabPage_ABC";
            this.tabPage_ABC.Size = new System.Drawing.Size(714, 472);
            this.tabPage_ABC.TabIndex = 2;
            this.tabPage_ABC.Text = "ABC Browser";
            // 
            // splitContainer_abc
            // 
            this.splitContainer_abc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_abc.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_abc.Name = "splitContainer_abc";
            // 
            // splitContainer_abc.Panel1
            // 
            this.splitContainer_abc.Panel1.Controls.Add(this.abcBrowser_abc);
            this.splitContainer_abc.Panel2Collapsed = true;
            this.splitContainer_abc.Size = new System.Drawing.Size(714, 472);
            this.splitContainer_abc.SplitterDistance = 486;
            this.splitContainer_abc.TabIndex = 0;
            // 
            // abcBrowser_abc
            // 
            this.abcBrowser_abc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.abcBrowser_abc.Location = new System.Drawing.Point(0, 0);
            this.abcBrowser_abc.MinimumSize = new System.Drawing.Size(20, 20);
            this.abcBrowser_abc.Name = "abcBrowser_abc";
            this.abcBrowser_abc.Size = new System.Drawing.Size(714, 472);
            this.abcBrowser_abc.TabIndex = 0;
            // 
            // imageList_tab
            // 
            this.imageList_tab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_tab.ImageStream")));
            this.imageList_tab.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_tab.Images.SetKeyName(0, "abc.png");
            this.imageList_tab.Images.SetKeyName(1, "gcm_logo.png");
            this.imageList_tab.Images.SetKeyName(2, "local.png");
            // 
            // GCMProView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcmTabControl_GCM);
            this.Name = "GCMProView";
            this.Size = new System.Drawing.Size(716, 513);
            this.gcmTabControl_GCM.ResumeLayout(false);
            this.tabPageEx_Local.ResumeLayout(false);
            this.splitContainer_local.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_local)).EndInit();
            this.splitContainer_local.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dcmDataGridView_local)).EndInit();
            this.tabPageEx_GCM.ResumeLayout(false);
            this.splitContainer_GCM.Panel1.ResumeLayout(false);
            this.splitContainer_GCM.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_GCM)).EndInit();
            this.splitContainer_GCM.ResumeLayout(false);
            this.panel_GCM_start.ResumeLayout(false);
            this.panel_GCM_start.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Signhelp)).EndInit();
            this.splitContainer_GCMData.Panel1.ResumeLayout(false);
            this.splitContainer_GCMData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_GCMData)).EndInit();
            this.splitContainer_GCMData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dcmDataGridView_gcm)).EndInit();
            this.tabPage_ABC.ResumeLayout(false);
            this.splitContainer_abc.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_abc)).EndInit();
            this.splitContainer_abc.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DCMControlLib.GCM.GCMTabControl gcmTabControl_GCM;
        private DCMControlLib.GCM.TabPageEx tabPageEx_Local;
        private DCMControlLib.GCM.TabPageEx tabPageEx_GCM;
        private DCMControlLib.GCM.TabPageEx tabPage_ABC;
        private System.Windows.Forms.ImageList imageList_tab;
        private System.Windows.Forms.SplitContainer splitContainer_local;
        private System.Windows.Forms.SplitContainer splitContainer_GCM;
        private System.Windows.Forms.SplitContainer splitContainer_abc;
        private DCMControlLib.DCMDataGridView dcmDataGridView_local;
        private DCMControlLib.DCMDataGridView dcmDataGridView_gcm;
        private DCMControlLib.GCM.ABCBrowser abcBrowser_abc;
        private System.Windows.Forms.SplitContainer splitContainer_GCMData;
        private System.Windows.Forms.Panel panel_GCM_start;
        private System.Windows.Forms.PictureBox pictureBox_Signhelp;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.CheckBox checkBox_remember;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.TextBox textBox_pwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ccinfoId;
        private System.Windows.Forms.Label label_user;
        private DCMControlLib.Tree.DCMTreeView dcmTreeView_gcm;
        private System.Windows.Forms.ImageList imageList_gcmtree;
    }
}
