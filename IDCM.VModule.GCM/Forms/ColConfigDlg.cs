using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Core;

namespace IDCM.Forms
{
    public partial class ColConfigDlg : Form
    {
        #region Constructor&Destructor

        public ColConfigDlg(int cursor, CustomColDef ccd)
        {
            InitializeComponent();
            this.label_name.Text = IDCM.Base.GlobalTextRes.Text("Column Name")+":";
            this.label_alias.Text = IDCM.Base.GlobalTextRes.Text("Column Alias") + ":";
            this.label4.Text = IDCM.Base.GlobalTextRes.Text("Restrict Expression")+":";
            this.checkBox_NotEmpty.Text = IDCM.Base.GlobalTextRes.Text("Not Empty");
            this.checkBox_unique.Text = IDCM.Base.GlobalTextRes.Text("Unique");
            this.Text = IDCM.Base.GlobalTextRes.Text("Column Config Dialog");
            this.label_defaultVal.Text = IDCM.Base.GlobalTextRes.Text("Default Value")+":";
            //////////////////////////////////////////////
            this.cursor = cursor;
            this.customCol = ccd;
            this.label_colName.Text = ccd.Attr;
            this.textBox_ColAlias.Text = ccd.Alias;
            this.checkBox_NotEmpty.Checked = ccd.IsRequire;
            this.checkBox_unique.Checked = ccd.IsUnique;
            this.textBox_defaultVal.Text = ccd.DefaultVal;
            this.textBox_restrict.Text = ccd.Restrict;
            this.textBox_restrict.BackColor = Color.White;
            dirtyStatus = false;
            this.FormClosing+=ColConfigDlg_FormClosing;
        }
        #endregion

        #region Events&Handlings
        public event ColConfigChangedHandler ColConfigChanged;

        private void ColConfigDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason.Equals(CloseReason.UserClosing) && ColConfigChanged != null)
            {
                if (this.textBox_restrict.BackColor.Equals(Color.White))
                {
                    if (dirtyStatus)
                    {
                        this.customCol.IsRequire = this.checkBox_NotEmpty.Checked;
                        this.customCol.IsUnique = this.checkBox_unique.Checked;
                        this.customCol.Restrict = this.textBox_restrict.Text;
                        this.customCol.Alias = this.textBox_ColAlias.Text;
                        this.customCol.DefaultVal = this.textBox_defaultVal.Text;
                        ColConfigChanged(cursor, customCol);
                    }
                }
                else
                {
                    e.Cancel = true;
                    this.textBox_restrict.Focus();
                }
            }
        }

        private void checkBox_NotEmpty_CheckedChanged(object sender, EventArgs e)
        {
            customCol.IsRequire = this.checkBox_NotEmpty.Checked;
            dirtyStatus = true;
        }

        private void checkBox_unique_CheckedChanged(object sender, EventArgs e)
        {
            customCol.IsUnique = this.checkBox_unique.Checked;
            dirtyStatus = true;
        }
        private void textBox_defaultVal_TextChanged(object sender, EventArgs e)
        {
            customCol.DefaultVal = this.textBox_defaultVal.Text;
            dirtyStatus = true;
        }

        private void textBox_ColAlias_TextChanged(object sender, EventArgs e)
        {
            customCol.Alias = this.textBox_ColAlias.Text;
            dirtyStatus = true;
        }
        private void textBox_restrict_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCMEx.DCMEx dcmEx = new DCMEx.DCMEx(this.textBox_restrict.Text);
                customCol.Restrict = this.textBox_restrict.Text;
                this.textBox_restrict.BackColor = Color.White;
                dirtyStatus = true;
            }
            catch (Exception)
            {
                this.textBox_restrict.BackColor = Color.Red;
            }
        }
        #endregion

        #region Methods

        internal void Show(Form parent,Point point)
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Location = point;
            this.ShowDialog(parent);
        }
        #endregion

        #region Members

        private int cursor = -1;
        private CustomColDef customCol;

        private bool dirtyStatus = false;
        public delegate void ColConfigChangedHandler(int cursor,CustomColDef ccd);
        #endregion
    }
}
