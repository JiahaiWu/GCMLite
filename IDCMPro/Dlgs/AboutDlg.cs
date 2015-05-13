using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Dlgs
{
    public partial class AboutDlg : Form
    {
        #region Constructor&Destructor
        public AboutDlg()
        {
            InitializeComponent();
            setAboutText();
        }
        #endregion

        #region Events&Handlings
        private void button1_Click(object sender, EventArgs e)
        {
            if(!this.IsDisposed)
            this.Dispose();
        }
        #endregion

        #region Methods
        public void setAboutText()
        {
            this.version.Text = "IDCM v1.0(110)\n\nCopyright © All Rights Reserved";
            this.contact.Text = "Contact: jiahaiWu \n\n ";
            this.email.Text = "Email:jiahaiwu@im.ac.cn\n\n";
            this.address.Text = "Address:Beijing Chaoyang District";
        }
        #endregion
    }
}
