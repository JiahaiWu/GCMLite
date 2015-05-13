using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Forms
{
    public partial class ColConfigDlg : Form
    {
        public ColConfigDlg(int cursor, string aliasName, bool isRequire, bool isUnique, string restrict)
        {
            InitializeComponent();
            this.label1.Text = IDCM.Base.GlobalTextRes.Text("Column Name")+":";
            this.label4.Text = IDCM.Base.GlobalTextRes.Text("Restrict Expression")+":";
            this.checkBox_NotEmpty.Text = IDCM.Base.GlobalTextRes.Text("Not Empty");
            this.checkBox_unique.Text = IDCM.Base.GlobalTextRes.Text("Unique");
            this.Text = IDCM.Base.GlobalTextRes.Text("Column Config Dialog");
            //////////////////////////////////////////////
            this.cursor = cursor;
            this.label_ColName.Text = aliasName;
            this.checkBox_NotEmpty.Checked = isRequire;
            this.checkBox_unique.Checked = isUnique;
            this.textBox_restrict.Text = restrict;
            this.textBox_restrict.BackColor = Color.White;
            dirtyStatus = false;
            this.FormClosing+=ColConfigDlg_FormClosing;
        }

        private void ColConfigDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason.Equals(CloseReason.UserClosing) && ColConfigChanged != null)
            {
                if (this.textBox_restrict.BackColor.Equals(Color.White))
                {
                    if (dirtyStatus)
                    {
                        this.isRequire = this.checkBox_NotEmpty.Checked;
                        this.isUnique = this.checkBox_unique.Checked;
                        this.restrictVal = this.textBox_restrict.Text;
                        ColConfigChanged(cursor, isRequire, isUnique, restrictVal);
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
            this.isRequire = this.checkBox_NotEmpty.Checked;
            dirtyStatus = true;
        }

        private void checkBox_unique_CheckedChanged(object sender, EventArgs e)
        {
            this.isUnique = this.checkBox_unique.Checked;
            dirtyStatus = true;
        }

        private void textBox_restrict_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCMEx.DCMEx dcmEx = new DCMEx.DCMEx(this.textBox_restrict.Text);
                this.restrictVal = this.textBox_restrict.Text;
                this.textBox_restrict.BackColor = Color.White;
                dirtyStatus = true;
            }
            catch (Exception)
            {
                this.textBox_restrict.BackColor = Color.Red;
            }
        }

        internal void Show(Form parent,Point point)
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Location = point;
            this.ShowDialog(parent);
        }
        private int cursor = -1;
        private bool isRequire = false;
        private bool isUnique = false;
        private string restrictVal = null;
        public event ColConfigChangedHandler ColConfigChanged;
        private bool dirtyStatus = false;
        public delegate void ColConfigChangedHandler(int cursor, bool isRequire, bool isUnique, string restrict);
    }
}
