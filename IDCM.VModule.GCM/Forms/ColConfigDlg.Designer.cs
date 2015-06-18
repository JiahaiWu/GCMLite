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
            this.label_alias = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox_NotEmpty = new System.Windows.Forms.CheckBox();
            this.textBox_restrict = new System.Windows.Forms.TextBox();
            this.checkBox_unique = new System.Windows.Forms.CheckBox();
            this.label_name = new System.Windows.Forms.Label();
            this.label_colName = new System.Windows.Forms.Label();
            this.textBox_ColAlias = new System.Windows.Forms.TextBox();
            this.label_defaultVal = new System.Windows.Forms.Label();
            this.textBox_defaultVal = new System.Windows.Forms.TextBox();
            this.label_comments = new System.Windows.Forms.Label();
            this.textBox_comments = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label_alias, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_NotEmpty, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox_restrict, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_unique, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label_name, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_colName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_ColAlias, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_defaultVal, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBox_defaultVal, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label_comments, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.textBox_comments, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(525, 266);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label_alias
            // 
            this.label_alias.AutoSize = true;
            this.label_alias.Dock = System.Windows.Forms.DockStyle.Right;
            this.label_alias.Location = new System.Drawing.Point(15, 27);
            this.label_alias.Name = "label_alias";
            this.label_alias.Size = new System.Drawing.Size(83, 25);
            this.label_alias.TabIndex = 1;
            this.label_alias.Text = "Column Alias:";
            this.label_alias.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(27, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 112);
            this.label4.TabIndex = 4;
            this.label4.Text = "Restrict Expression:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBox_NotEmpty
            // 
            this.checkBox_NotEmpty.AutoSize = true;
            this.checkBox_NotEmpty.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox_NotEmpty.Location = new System.Drawing.Point(105, 56);
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
            this.textBox_restrict.Location = new System.Drawing.Point(103, 132);
            this.textBox_restrict.Margin = new System.Windows.Forms.Padding(1);
            this.textBox_restrict.Multiline = true;
            this.textBox_restrict.Name = "textBox_restrict";
            this.textBox_restrict.Size = new System.Drawing.Size(420, 110);
            this.textBox_restrict.TabIndex = 8;
            this.textBox_restrict.TextChanged += new System.EventHandler(this.textBox_restrict_TextChanged);
            // 
            // checkBox_unique
            // 
            this.checkBox_unique.AutoSize = true;
            this.checkBox_unique.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox_unique.Location = new System.Drawing.Point(105, 82);
            this.checkBox_unique.Name = "checkBox_unique";
            this.checkBox_unique.Size = new System.Drawing.Size(60, 19);
            this.checkBox_unique.TabIndex = 7;
            this.checkBox_unique.Text = "Unique";
            this.checkBox_unique.UseVisualStyleBackColor = true;
            this.checkBox_unique.CheckedChanged += new System.EventHandler(this.checkBox_unique_CheckedChanged);
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Dock = System.Windows.Forms.DockStyle.Right;
            this.label_name.Location = new System.Drawing.Point(21, 1);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(77, 25);
            this.label_name.TabIndex = 9;
            this.label_name.Text = "Column Name:";
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_colName
            // 
            this.label_colName.AutoSize = true;
            this.label_colName.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_colName.Location = new System.Drawing.Point(105, 1);
            this.label_colName.Name = "label_colName";
            this.label_colName.Size = new System.Drawing.Size(11, 25);
            this.label_colName.TabIndex = 10;
            this.label_colName.Text = "?";
            this.label_colName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_ColAlias
            // 
            this.textBox_ColAlias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_ColAlias.Location = new System.Drawing.Point(103, 28);
            this.textBox_ColAlias.Margin = new System.Windows.Forms.Padding(1);
            this.textBox_ColAlias.Name = "textBox_ColAlias";
            this.textBox_ColAlias.Size = new System.Drawing.Size(420, 21);
            this.textBox_ColAlias.TabIndex = 11;
            this.textBox_ColAlias.TextChanged += new System.EventHandler(this.textBox_ColAlias_TextChanged);
            // 
            // label_defaultVal
            // 
            this.label_defaultVal.AutoSize = true;
            this.label_defaultVal.Dock = System.Windows.Forms.DockStyle.Right;
            this.label_defaultVal.Location = new System.Drawing.Point(9, 105);
            this.label_defaultVal.Name = "label_defaultVal";
            this.label_defaultVal.Size = new System.Drawing.Size(89, 25);
            this.label_defaultVal.TabIndex = 12;
            this.label_defaultVal.Text = "Default Value:";
            this.label_defaultVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_defaultVal
            // 
            this.textBox_defaultVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_defaultVal.Location = new System.Drawing.Point(103, 106);
            this.textBox_defaultVal.Margin = new System.Windows.Forms.Padding(1);
            this.textBox_defaultVal.Name = "textBox_defaultVal";
            this.textBox_defaultVal.Size = new System.Drawing.Size(420, 21);
            this.textBox_defaultVal.TabIndex = 13;
            this.textBox_defaultVal.TextChanged += new System.EventHandler(this.textBox_defaultVal_TextChanged);
            // 
            // label_comments
            // 
            this.label_comments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_comments.AutoSize = true;
            this.label_comments.Location = new System.Drawing.Point(39, 244);
            this.label_comments.Name = "label_comments";
            this.label_comments.Size = new System.Drawing.Size(59, 25);
            this.label_comments.TabIndex = 14;
            this.label_comments.Text = "Comments:";
            this.label_comments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_comments
            // 
            this.textBox_comments.BackColor = System.Drawing.Color.LightCyan;
            this.textBox_comments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_comments.Location = new System.Drawing.Point(102, 244);
            this.textBox_comments.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_comments.Multiline = true;
            this.textBox_comments.Name = "textBox_comments";
            this.textBox_comments.Size = new System.Drawing.Size(422, 25);
            this.textBox_comments.TabIndex = 15;
            this.textBox_comments.TextChanged += new System.EventHandler(this.textBox_comments_TextChanged);
            // 
            // ColConfigDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 266);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColConfigDlg";
            this.Text = "Column Config Dialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_alias;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_NotEmpty;
        private System.Windows.Forms.TextBox textBox_restrict;
        private System.Windows.Forms.CheckBox checkBox_unique;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_colName;
        private System.Windows.Forms.TextBox textBox_ColAlias;
        private System.Windows.Forms.Label label_defaultVal;
        private System.Windows.Forms.TextBox textBox_defaultVal;
        private System.Windows.Forms.Label label_comments;
        private System.Windows.Forms.TextBox textBox_comments;
    }
}