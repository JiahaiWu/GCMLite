namespace IDCM.Dlgs
{
    partial class GCMLiteScaffold
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GCMLiteScaffold));
            this.button_i18n_EN = new System.Windows.Forms.Button();
            this.button_i18N_CN = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.checkBox_noMapping = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_i18n_EN
            // 
            this.button_i18n_EN.Location = new System.Drawing.Point(182, 22);
            this.button_i18n_EN.Name = "button_i18n_EN";
            this.button_i18n_EN.Size = new System.Drawing.Size(142, 23);
            this.button_i18n_EN.TabIndex = 0;
            this.button_i18n_EN.Text = "i18N_EN Recalling";
            this.button_i18n_EN.UseVisualStyleBackColor = true;
            this.button_i18n_EN.Click += new System.EventHandler(this.button_i18n_EN_Click);
            // 
            // button_i18N_CN
            // 
            this.button_i18N_CN.Location = new System.Drawing.Point(361, 22);
            this.button_i18N_CN.Name = "button_i18N_CN";
            this.button_i18N_CN.Size = new System.Drawing.Size(150, 23);
            this.button_i18N_CN.TabIndex = 1;
            this.button_i18N_CN.Text = "i18N_CN Recalling";
            this.button_i18N_CN.UseVisualStyleBackColor = true;
            this.button_i18N_CN.Click += new System.EventHandler(this.button_i18N_CN_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 82);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(550, 290);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // checkBox_noMapping
            // 
            this.checkBox_noMapping.AutoSize = true;
            this.checkBox_noMapping.Location = new System.Drawing.Point(52, 29);
            this.checkBox_noMapping.Name = "checkBox_noMapping";
            this.checkBox_noMapping.Size = new System.Drawing.Size(114, 16);
            this.checkBox_noMapping.TabIndex = 3;
            this.checkBox_noMapping.Text = "Only no mapping";
            this.checkBox_noMapping.UseVisualStyleBackColor = true;
            // 
            // GCMLiteScaffold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 372);
            this.Controls.Add(this.checkBox_noMapping);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button_i18N_CN);
            this.Controls.Add(this.button_i18n_EN);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GCMLiteScaffold";
            this.Text = "GCMLiteScaffold";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_i18n_EN;
        private System.Windows.Forms.Button button_i18N_CN;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox_noMapping;
    }
}