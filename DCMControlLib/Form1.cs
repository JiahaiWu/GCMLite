using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCMControlLib
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create rich textbox control.
            RichTextBox richTextBox = new RichTextBox();
            this.dcmComboBox1.DropDownControl = richTextBox;
        }

        private void dcmComboBox1_DropDown(object sender, EventArgs e)
        {
            dcmComboBox1.ValueStatus = DCMComboBox.ValidationStatus.Error;
        }

        private void dcmComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            //dcmComboBox1.ValueStatus = DCMComboBox.ValidationStatus.Ok;
        }
    }
}
