using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCM.ComPO
{
    internal class SignVModel
    {
        public SignVModel(System.Windows.Forms.Control.ControlCollection controlCollection)
        {
            ccinfoIDBox = controlCollection["textBox_ccinfoId"] as TextBox;
            gcmSignPwd = controlCollection["textBox_pwd"] as TextBox;
            rememberPwd = controlCollection["checkBox_remember"] as CheckBox;
        }
        public string CCInfoID
        {
            get
            {
                return this.ccinfoIDBox.Text.Trim();
            }
            set
            {
                this.ccinfoIDBox.Text = value != null ? value : "";
            }
        }
        public string SignPwd
        {
            get
            {
                return this.gcmSignPwd.Text.Trim();
            }
            set
            {
                this.gcmSignPwd.Text = value!=null?value:"";
            }
        }
        public bool isRemember
        {
            get
            {
                return this.rememberPwd.Checked;
            }
            set
            {
                this.rememberPwd.Checked=value;
            }
        }

        private TextBox ccinfoIDBox = null;
        private TextBox gcmSignPwd = null;
        private CheckBox rememberPwd = null;
    }
}
