namespace DCMControlLib.GCM
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gcmTabControl1 = new DCMControlLib.GCM.GCMTabControl();
            this.tabPageEx1 = new DCMControlLib.GCM.TabPageEx();
            this.tabPageEx2 = new DCMControlLib.GCM.TabPageEx();
            this.tabPageEx3 = new DCMControlLib.GCM.TabPageEx();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.gcmTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcmTabControl1
            // 
            this.gcmTabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.gcmTabControl1.Alignments = DCMControlLib.GCM.GCMTabControl.TabAlignments.Bottom;
            this.gcmTabControl1.AllowDrop = true;
            this.gcmTabControl1.BackgroundHatcher.HatchType = System.Drawing.Drawing2D.HatchStyle.DashedVertical;
            this.gcmTabControl1.BorderColor = System.Drawing.Color.SkyBlue;
            this.gcmTabControl1.Controls.Add(this.tabPageEx1);
            this.gcmTabControl1.Controls.Add(this.tabPageEx2);
            this.gcmTabControl1.Controls.Add(this.tabPageEx3);
            this.gcmTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcmTabControl1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gcmTabControl1.HeaderStyle = DCMControlLib.GCM.GCMTabControl.TabHeaderStyle.Solid;
            this.gcmTabControl1.ImageList = this.imageList1;
            this.gcmTabControl1.IsCaptionVisible = false;
            this.gcmTabControl1.IsDrawHeader = false;
            this.gcmTabControl1.ItemSize = new System.Drawing.Size(0, 30);
            this.gcmTabControl1.Location = new System.Drawing.Point(0, 0);
            this.gcmTabControl1.Name = "gcmTabControl1";
            this.gcmTabControl1.SelectedIndex = 0;
            this.gcmTabControl1.Size = new System.Drawing.Size(699, 366);
            this.gcmTabControl1.TabGradient.ColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(223)))), ((int)(((byte)(246)))));
            this.gcmTabControl1.TabGradient.ColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(67)))), ((int)(((byte)(164)))));
            this.gcmTabControl1.TabGradient.GradientStyle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gcmTabControl1.TabGradient.TabPageSelectedTextColor = System.Drawing.Color.White;
            this.gcmTabControl1.TabGradient.TabPageTextColor = System.Drawing.Color.DimGray;
            this.gcmTabControl1.TabHOffset = -2;
            this.gcmTabControl1.TabIndex = 0;
            this.gcmTabControl1.TabPageCloseIconColor = System.Drawing.Color.White;
            this.gcmTabControl1.UpDownStyle = DCMControlLib.GCM.GCMTabControl.UpDown32Style.KRBBlue;
            // 
            // tabPageEx1
            // 
            this.tabPageEx1.BackColor = System.Drawing.Color.White;
            this.tabPageEx1.Font = new System.Drawing.Font("Arial", 10F);
            this.tabPageEx1.ImageIndex = 0;
            this.tabPageEx1.IsClosable = false;
            this.tabPageEx1.Location = new System.Drawing.Point(1, 1);
            this.tabPageEx1.Name = "tabPageEx1";
            this.tabPageEx1.Size = new System.Drawing.Size(697, 329);
            this.tabPageEx1.TabIndex = 0;
            this.tabPageEx1.Text = "tabPageEx1";
            // 
            // tabPageEx2
            // 
            this.tabPageEx2.BackColor = System.Drawing.Color.White;
            this.tabPageEx2.Font = new System.Drawing.Font("Arial", 10F);
            this.tabPageEx2.ImageIndex = 1;
            this.tabPageEx2.IsClosable = false;
            this.tabPageEx2.Location = new System.Drawing.Point(1, 1);
            this.tabPageEx2.Name = "tabPageEx2";
            this.tabPageEx2.Size = new System.Drawing.Size(697, 329);
            this.tabPageEx2.TabIndex = 1;
            this.tabPageEx2.Text = "tabPageEx2";
            // 
            // tabPageEx3
            // 
            this.tabPageEx3.BackColor = System.Drawing.Color.White;
            this.tabPageEx3.Font = new System.Drawing.Font("Arial", 10F);
            this.tabPageEx3.ImageIndex = 2;
            this.tabPageEx3.IsClosable = false;
            this.tabPageEx3.Location = new System.Drawing.Point(1, 1);
            this.tabPageEx3.Name = "tabPageEx3";
            this.tabPageEx3.Size = new System.Drawing.Size(697, 329);
            this.tabPageEx3.TabIndex = 2;
            this.tabPageEx3.Text = "tabPageEx3";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "local.png");
            this.imageList1.Images.SetKeyName(1, "logo.png");
            this.imageList1.Images.SetKeyName(2, "online.png");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 366);
            this.Controls.Add(this.gcmTabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gcmTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GCMTabControl gcmTabControl1;
        private TabPageEx tabPageEx1;
        private TabPageEx tabPageEx2;
        private TabPageEx tabPageEx3;
        private System.Windows.Forms.ImageList imageList1;
    }
}

