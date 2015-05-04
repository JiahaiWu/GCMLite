using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM
{
    public partial class DCMPopupTmpl : UserControl
    {
        public DCMPopupTmpl()
        {
            InitializeComponent();
        }

        private void UserControl1_SizeChanged(object sender, EventArgs e)
        {
            // Calculate size of tableLayoutPanel box.
            tableLayoutPanel1.Size = new Size(DisplayRectangle.Width - tableLayoutPanel1.Left - 5,
                                            DisplayRectangle.Height - tableLayoutPanel1.Top - button_confirm.Height - 10);
        }
    }
}
