namespace IDCM.Forms
{
    partial class ConfigColumnsDlg
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView_colCfg = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_Check = new System.Windows.Forms.Button();
            this.Btn_Confirm = new System.Windows.Forms.Button();
            this.Attr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unique = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Require = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Restrict = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefaultVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Enable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Up = new System.Windows.Forms.DataGridViewImageColumn();
            this.Down = new System.Windows.Forms.DataGridViewImageColumn();
            this.Delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_colCfg)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView_colCfg, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(932, 437);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView_colCfg
            // 
            this.dataGridView_colCfg.AllowUserToOrderColumns = true;
            this.dataGridView_colCfg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_colCfg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Attr,
            this.Unique,
            this.Require,
            this.Restrict,
            this.Alias,
            this.DefaultVal,
            this.Enable,
            this.Up,
            this.Down,
            this.Delete});
            this.dataGridView_colCfg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_colCfg.Location = new System.Drawing.Point(3, 53);
            this.dataGridView_colCfg.Name = "dataGridView_colCfg";
            this.dataGridView_colCfg.RowTemplate.Height = 23;
            this.dataGridView_colCfg.Size = new System.Drawing.Size(926, 381);
            this.dataGridView_colCfg.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Btn_Cancel);
            this.panel1.Controls.Add(this.Btn_Check);
            this.panel1.Controls.Add(this.Btn_Confirm);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(926, 44);
            this.panel1.TabIndex = 1;
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Cancel.Location = new System.Drawing.Point(618, 9);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Btn_Cancel.TabIndex = 2;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Btn_Check
            // 
            this.Btn_Check.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Check.Location = new System.Drawing.Point(727, 9);
            this.Btn_Check.Name = "Btn_Check";
            this.Btn_Check.Size = new System.Drawing.Size(75, 23);
            this.Btn_Check.TabIndex = 1;
            this.Btn_Check.Text = "Check";
            this.Btn_Check.UseVisualStyleBackColor = true;
            this.Btn_Check.Click += new System.EventHandler(this.Check_Click);
            // 
            // Btn_Confirm
            // 
            this.Btn_Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Confirm.Location = new System.Drawing.Point(829, 9);
            this.Btn_Confirm.Name = "Btn_Confirm";
            this.Btn_Confirm.Size = new System.Drawing.Size(75, 23);
            this.Btn_Confirm.TabIndex = 0;
            this.Btn_Confirm.Text = "Confirm";
            this.Btn_Confirm.UseVisualStyleBackColor = true;
            this.Btn_Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Attr
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Attr.DefaultCellStyle = dataGridViewCellStyle3;
            this.Attr.HeaderText = "Name";
            this.Attr.Name = "Attr";
            // 
            // Unique
            // 
            this.Unique.HeaderText = "Unique";
            this.Unique.Name = "Unique";
            // 
            // Require
            // 
            this.Require.HeaderText = "Require";
            this.Require.Name = "Require";
            // 
            // Restrict
            // 
            this.Restrict.HeaderText = "Restrict";
            this.Restrict.Name = "Restrict";
            // 
            // Alias
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Alias.DefaultCellStyle = dataGridViewCellStyle4;
            this.Alias.HeaderText = "Alias";
            this.Alias.Name = "Alias";
            // 
            // DefaultVal
            // 
            this.DefaultVal.HeaderText = "Default Value";
            this.DefaultVal.Name = "DefaultVal";
            // 
            // Enable
            // 
            this.Enable.HeaderText = "Enable";
            this.Enable.Name = "Enable";
            // 
            // Up
            // 
            this.Up.HeaderText = "Up";
            this.Up.Image = global::IDCM.Properties.Resources.up;
            this.Up.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Up.Name = "Up";
            this.Up.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Up.Width = 40;
            // 
            // Down
            // 
            this.Down.HeaderText = "Down";
            this.Down.Image = global::IDCM.Properties.Resources.down;
            this.Down.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Down.Name = "Down";
            this.Down.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Down.Width = 40;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Image = global::IDCM.Properties.Resources.del_note;
            this.Delete.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Delete.Name = "Delete";
            this.Delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Delete.Width = 50;
            // 
            // ConfigColumnsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 437);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ConfigColumnsDlg";
            this.Text = "ConfigColumnsDlg";
            this.Load += new System.EventHandler(this.ConfigColumnsDlg_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_colCfg)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView_colCfg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_Check;
        private System.Windows.Forms.Button Btn_Confirm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attr;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Unique;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Require;
        private System.Windows.Forms.DataGridViewTextBoxColumn Restrict;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultVal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enable;
        private System.Windows.Forms.DataGridViewImageColumn Up;
        private System.Windows.Forms.DataGridViewImageColumn Down;
        private System.Windows.Forms.DataGridViewImageColumn Delete;
    }
}