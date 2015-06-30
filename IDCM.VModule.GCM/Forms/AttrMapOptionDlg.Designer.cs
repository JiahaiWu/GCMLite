namespace IDCM.Forms
{
    partial class AttrMapOptionDlg
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttrMapOptionDlg));
            this.dataGridView_map = new System.Windows.Forms.DataGridView();
            this.Column_src = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_tag = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column_dest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_UnBind = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column_rebind = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_confirm = new System.Windows.Forms.Button();
            this.radioButton_custom = new System.Windows.Forms.RadioButton();
            this.radioButton_exact = new System.Windows.Forms.RadioButton();
            this.radioButton_similarity = new System.Windows.Forms.RadioButton();
            this.button_cancel = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.label_designateScope = new System.Windows.Forms.Label();
            this.comboBox_designateScope = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_map)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_map
            // 
            this.dataGridView_map.AllowUserToAddRows = false;
            this.dataGridView_map.AllowUserToDeleteRows = false;
            this.dataGridView_map.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView_map.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_map.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_map.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_src,
            this.Column_tag,
            this.Column_dest,
            this.Column_UnBind,
            this.Column_rebind});
            this.dataGridView_map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_map.Location = new System.Drawing.Point(3, 83);
            this.dataGridView_map.Name = "dataGridView_map";
            this.dataGridView_map.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_map.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_map.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView_map.RowTemplate.Height = 23;
            this.dataGridView_map.Size = new System.Drawing.Size(710, 429);
            this.dataGridView_map.TabIndex = 0;
            this.dataGridView_map.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_map_CellMouseClick);
            this.dataGridView_map.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_map_RowPostPaint);
            // 
            // Column_src
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_src.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_src.HeaderText = "From Attr";
            this.Column_src.Name = "Column_src";
            this.Column_src.ReadOnly = true;
            this.Column_src.Width = 150;
            // 
            // Column_tag
            // 
            this.Column_tag.HeaderText = "To";
            this.Column_tag.Image = global::IDCM.Properties.Resources.rightArrow;
            this.Column_tag.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Column_tag.Name = "Column_tag";
            this.Column_tag.ReadOnly = true;
            // 
            // Column_dest
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_dest.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column_dest.HeaderText = "Dest Attr";
            this.Column_dest.Name = "Column_dest";
            this.Column_dest.ReadOnly = true;
            this.Column_dest.Width = 150;
            // 
            // Column_UnBind
            // 
            this.Column_UnBind.HeaderText = "Unbind";
            this.Column_UnBind.Image = global::IDCM.Properties.Resources.broken;
            this.Column_UnBind.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Column_UnBind.Name = "Column_UnBind";
            this.Column_UnBind.ReadOnly = true;
            // 
            // Column_rebind
            // 
            this.Column_rebind.HeaderText = "Rebind";
            this.Column_rebind.Name = "Column_rebind";
            this.Column_rebind.ReadOnly = true;
            this.Column_rebind.Text = "...";
            this.Column_rebind.ToolTipText = "...";
            this.Column_rebind.UseColumnTextForButtonValue = true;
            this.Column_rebind.Width = 60;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView_map, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(716, 515);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(710, 74);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_designateScope);
            this.groupBox1.Controls.Add(this.label_designateScope);
            this.groupBox1.Controls.Add(this.button_confirm);
            this.groupBox1.Controls.Add(this.radioButton_custom);
            this.groupBox1.Controls.Add(this.radioButton_exact);
            this.groupBox1.Controls.Add(this.radioButton_similarity);
            this.groupBox1.Controls.Add(this.button_cancel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 74);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(592, 41);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 4;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // radioButton_custom
            // 
            this.radioButton_custom.AutoSize = true;
            this.radioButton_custom.Location = new System.Drawing.Point(280, 44);
            this.radioButton_custom.Name = "radioButton_custom";
            this.radioButton_custom.Size = new System.Drawing.Size(107, 16);
            this.radioButton_custom.TabIndex = 3;
            this.radioButton_custom.Text = "Custom Mapping";
            this.radioButton_custom.UseVisualStyleBackColor = true;
            // 
            // radioButton_exact
            // 
            this.radioButton_exact.AutoSize = true;
            this.radioButton_exact.Location = new System.Drawing.Point(174, 44);
            this.radioButton_exact.Name = "radioButton_exact";
            this.radioButton_exact.Size = new System.Drawing.Size(89, 16);
            this.radioButton_exact.TabIndex = 2;
            this.radioButton_exact.Text = "Exact Match";
            this.radioButton_exact.UseVisualStyleBackColor = true;
            this.radioButton_exact.CheckedChanged += new System.EventHandler(this.radioButton_exact_CheckedChanged);
            // 
            // radioButton_similarity
            // 
            this.radioButton_similarity.AutoSize = true;
            this.radioButton_similarity.Location = new System.Drawing.Point(38, 44);
            this.radioButton_similarity.Name = "radioButton_similarity";
            this.radioButton_similarity.Size = new System.Drawing.Size(119, 16);
            this.radioButton_similarity.TabIndex = 1;
            this.radioButton_similarity.Text = "Similarity Match";
            this.radioButton_similarity.UseVisualStyleBackColor = true;
            this.radioButton_similarity.CheckedChanged += new System.EventHandler(this.radioButton_similarity_CheckedChanged);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(482, 41);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 0;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "To";
            this.dataGridViewImageColumn1.Image = global::IDCM.Properties.Resources.rightArrow;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "Unbound";
            this.dataGridViewImageColumn2.Image = global::IDCM.Properties.Resources.broken;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            // 
            // label_designateScope
            // 
            this.label_designateScope.AutoSize = true;
            this.label_designateScope.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_designateScope.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_designateScope.Location = new System.Drawing.Point(38, 19);
            this.label_designateScope.Name = "label_designateScope";
            this.label_designateScope.Size = new System.Drawing.Size(104, 15);
            this.label_designateScope.TabIndex = 5;
            this.label_designateScope.Text = "Designate Scope:";
            // 
            // comboBox_designateScope
            // 
            this.comboBox_designateScope.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_designateScope.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_designateScope.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_designateScope.FormattingEnabled = true;
            this.comboBox_designateScope.Location = new System.Drawing.Point(146, 16);
            this.comboBox_designateScope.Name = "comboBox_designateScope";
            this.comboBox_designateScope.Size = new System.Drawing.Size(238, 23);
            this.comboBox_designateScope.TabIndex = 6;
            this.comboBox_designateScope.Text = "Default";
            // 
            // AttrMapOptionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 515);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttrMapOptionDlg";
            this.Text = "AttrMappingOptionDlg";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_map)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_map;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.RadioButton radioButton_custom;
        private System.Windows.Forms.RadioButton radioButton_exact;
        private System.Windows.Forms.RadioButton radioButton_similarity;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_src;
        private System.Windows.Forms.DataGridViewImageColumn Column_tag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_dest;
        private System.Windows.Forms.DataGridViewImageColumn Column_UnBind;
        private System.Windows.Forms.DataGridViewButtonColumn Column_rebind;
        private System.Windows.Forms.Label label_designateScope;
        private System.Windows.Forms.ComboBox comboBox_designateScope;

    }
}