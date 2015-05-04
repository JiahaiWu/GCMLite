namespace DCMControlLib
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
            this.dcmComboBox1 = new DCMControlLib.DCMComboBox();
            this.SuspendLayout();
            // 
            // dcmComboBox1
            // 
            this.dcmComboBox1.AllowResizeDropDown = true;
            this.dcmComboBox1.ControlSize = new System.Drawing.Size(1, 1);
            this.dcmComboBox1.DropDownControl = null;
            this.dcmComboBox1.DropSize = new System.Drawing.Size(121, 106);
            this.dcmComboBox1.Location = new System.Drawing.Point(67, 43);
            this.dcmComboBox1.Name = "dcmComboBox1";
            this.dcmComboBox1.Size = new System.Drawing.Size(170, 20);
            this.dcmComboBox1.TabIndex = 0;
            this.dcmComboBox1.Text = "dcmComboBox1";
            this.dcmComboBox1.ValueStatus = DCMControlLib.DCMComboBox.ValidationStatus.Error;
            this.dcmComboBox1.DropDown += new System.EventHandler(this.dcmComboBox1_DropDown);
            this.dcmComboBox1.DropDownClosed += new System.EventHandler(this.dcmComboBox1_DropDownClosed);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.dcmComboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DCMComboBox dcmComboBox1;
    }
}

