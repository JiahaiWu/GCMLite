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
    public partial class ConfigColumnsDlg : Form
    {
        public ConfigColumnsDlg()
        {
            InitializeComponent();
            dataGridView_colCfg.RowPostPaint+=dataGridView_colCfg_RowPostPaint;
        }

        private void ConfigColumnsDlg_Load(object sender, EventArgs e)
        {
            ICollection<CustomColDef> ccds=  CustomColDefGetter.getCustomTableDef();
            dataGridView_colCfg.Rows.Clear();
            foreach (CustomColDef ccd in ccds)
            {
                int idx=dataGridView_colCfg.Rows.Add();
                DataGridViewRow dgvr = dataGridView_colCfg.Rows[idx];
                dgvr.Cells["Attr"].Value = ccd.Attr;
                dgvr.Cells["Require"].Value = ccd.IsRequire;
                dgvr.Cells["Unique"].Value = ccd.IsUnique;
                dgvr.Cells["Restrict"].Value = ccd.Restrict;
                dgvr.Cells["DefaultVal"].Value = ccd.DefaultVal;
                dgvr.Cells["Alias"].Value = ccd.Alias;
                dgvr.Cells["Enable"].Value = ccd.IsEnable;
            }
        }
        private void dataGridView_colCfg_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_colCfg.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridView_colCfg.RowHeadersDefaultCellStyle.Font, rectangle,
                dataGridView_colCfg.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            dataGridView_colCfg.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void Check_Click(object sender, EventArgs e)
        {
            if (checkFields())
            {
                MessageBox.Show("");
            }
            else
            {
                MessageBox.Show("Please notice the error tips shown in the data grid view, you need to update the value.");
            }
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (checkFields())
            {
                submitSetting();
            }
            else
            {
                MessageBox.Show("Please notice the error tips shown in the data grid view, fix those errors and confirm again.");
            }
            this.Close();
            this.Dispose();
        }

        private bool checkFields()
        {
            throw new NotImplementedException();
        }

        private void submitSetting()
        {
            throw new NotImplementedException();
        }

    }
}
