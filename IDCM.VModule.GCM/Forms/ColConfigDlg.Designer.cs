namespace IDCM.Forms
{
    partial class ColConfigDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColConfigDlg));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ccv_title = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_ColName = new System.Windows.Forms.Label();
            this.checkBox_NotEmpty = new System.Windows.Forms.CheckBox();
            this.textBox_restrict = new System.Windows.Forms.TextBox();
            this.checkBox_unique = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ccv_title, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label_ColName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_NotEmpty, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox_restrict, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_unique, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(334, 212);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(20, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Column Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ccv_title
            // 
            this.ccv_title.AutoSize = true;
            this.ccv_title.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.SetColumnSpan(this.ccv_title, 2);
            this.ccv_title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ccv_title.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ccv_title.Location = new System.Drawing.Point(3, 0);
            this.ccv_title.Name = "ccv_title";
            this.ccv_title.Size = new System.Drawing.Size(328, 25);
            this.ccv_title.TabIndex = 0;
            this.ccv_title.Text = "Column Config Popup";
            this.ccv_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(26, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 137);
            this.label4.TabIndex = 4;
            this.label4.Text = "Restrict Expression:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_ColName
            // 
            this.label_ColName.AutoSize = true;
            this.label_ColName.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_ColName.Location = new System.Drawing.Point(103, 25);
            this.label_ColName.Name = "label_ColName";
            this.label_ColName.Size = new System.Drawing.Size(11, 25);
            this.label_ColName.TabIndex = 5;
            this.label_ColName.Text = "?";
            this.label_ColName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_NotEmpty
            // 
            this.checkBox_NotEmpty.AutoSize = true;
            this.checkBox_NotEmpty.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox_NotEmpty.Location = new System.Drawing.Point(103, 53);
            this.checkBox_NotEmpty.Name = "checkBox_NotEmpty";
            this.checkBox_NotEmpty.Size = new System.Drawing.Size(78, 19);
            this.checkBox_NotEmpty.TabIndex = 6;
            this.checkBox_NotEmpty.Text = "Not Empty";
            this.checkBox_NotEmpty.UseVisualStyleBackColor = true;
            this.checkBox_NotEmpty.CheckedChanged += new System.EventHandler(this.checkBox_NotEmpty_CheckedChanged);
            // 
            // textBox_restrict
            // 
            this.textBox_restrict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_restrict.Location = new System.Drawing.Point(103, 78);
            this.textBox_restrict.Multiline = true;
            this.textBox_restrict.Name = "textBox_restrict";
            this.textBox_restrict.Size = new System.Drawing.Size(228, 131);
            this.textBox_restrict.TabIndex = 8;
            this.textBox_restrict.TextChanged += new System.EventHandler(this.textBox_restrict_TextChanged);
            // 
            // checkBox_unique
            // 
            this.checkBox_unique.AutoSize = true;
            this.checkBox_unique.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBox_unique.Location = new System.Drawing.Point(37, 53);
            this.checkBox_unique.Name = "checkBox_unique";
            this.checkBox_unique.Size = new System.Drawing.Size(60, 19);
            this.checkBox_unique.TabIndex = 7;
            this.checkBox_unique.Text = "Unique";
            this.checkBox_unique.UseVisualStyleBackColor = true;
            this.checkBox_unique.CheckedChanged += new System.EventHandler(this.checkBox_unique_CheckedChanged);
            // 
            // ColConfigDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 212);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColConfigDlg";
            this.Text = "ColConfigDlg";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ccv_title;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_ColName;
        private System.Windows.Forms.CheckBox checkBox_NotEmpty;
        private System.Windows.Forms.TextBox textBox_restrict;
        private System.Windows.Forms.CheckBox checkBox_unique;
    }
}