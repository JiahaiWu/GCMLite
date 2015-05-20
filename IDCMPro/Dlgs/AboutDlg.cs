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
            this.version.Text = "IDCM v1.0(110)\nCopyright © All Rights Reserved";
            this.contact.Text = "Contact: (+86)010-64807462";
            this.email.Text = "Email: office@im.ac.cn";
            this.address.Text = "Address: NO.1 Beichen West Road, Chaoyang District, Beijing 100101";
        }
        #endregion
    }
}
