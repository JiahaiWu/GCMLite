using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Core;
using IDCM.MsgDriver;
using IDCM.Base;
using IDCM.BGHandlerManager;

namespace IDCM.Forms
{
    public partial class ConfigColumnsDlg : Form
    {
        public ConfigColumnsDlg()
        {
            InitializeComponent();
            dataGridView_colCfg.RowPostPaint+=dataGridView_colCfg_RowPostPaint;
            dataGridView_colCfg.CellClick += dataGridView_colCfg_CellClick;
        }

        void dataGridView_colCfg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            if (dataGridView_colCfg.Columns[e.ColumnIndex].HeaderText.Equals("Down"))
            {
                int selectedRowIndex = e.RowIndex;
                int rowCount = dataGridView_colCfg.Rows.Count - 1;
                if (dataGridView_colCfg.Rows[rowCount].IsNewRow)
                    --rowCount;
                if (selectedRowIndex < rowCount)
                {
                    DataGridViewRow newRow = dataGridView_colCfg.Rows[selectedRowIndex];
                    dataGridView_colCfg.Rows.RemoveAt(selectedRowIndex);
                    dataGridView_colCfg.Rows.Insert(selectedRowIndex + 1, newRow);
                    //dataGridView_fields.Rows[selectedRowIndex + 1].Selected = true;
                }
            }
            else if (dataGridView_colCfg.Columns[e.ColumnIndex].HeaderText.Equals("Up"))
            {
                int selectedRowIndex = e.RowIndex;
                if (selectedRowIndex > 0)
                {
                    DataGridViewRow newRow = dataGridView_colCfg.Rows[selectedRowIndex];
                    dataGridView_colCfg.Rows.RemoveAt(selectedRowIndex);
                    dataGridView_colCfg.Rows.Insert(selectedRowIndex - 1, newRow);
                    //dataGridView_fields.Rows[selectedRowIndex - 1].Selected = true;
                }
            }
            else if (dataGridView_colCfg.Columns[e.ColumnIndex].HeaderText.Equals("Delete"))
            {
                OnRemoveField(this, new IDCMViewEventArgs(new DataGridViewRow[] { dataGridView_colCfg.Rows[e.RowIndex] }));
                dataGridView_colCfg.Rows.RemoveAt(e.RowIndex);
            }
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
                MessageBox.Show("It seems all right.");
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


        private void comboBox_keyField_Click(object sender, EventArgs e)
        {
            List<string> iterms = new List<string>();
            foreach (DataGridViewRow dgvr in dataGridView_colCfg.Rows)
            {
                if(dgvr.IsNewRow)
                    continue;
                DataGridViewCell dgvc = dgvr.Cells["Attr"];
                if (dgvc != null && dgvc.Value != null)
                {
                    string term = dgvc.FormattedValue.ToString();
                    if(term.Length>0)
                        iterms.Add(term);
                }
            }
            comboBox_keyField.Items.Clear();
            comboBox_keyField.Items.AddRange(iterms.ToArray());
            //comboBox_keyField.Invalidate();
            //comboBox_keyField.Update();
        }

        private bool checkFields()
        {
            bool noError = true;
            //验证字段名重复 &
            //验证字段约束表达式
            HashSet<string> iterms = new HashSet<string>();
            foreach (DataGridViewRow dgvr in dataGridView_colCfg.Rows)
            {
                if (dgvr.IsNewRow)
                    continue;
                DataGridViewCell dgvc = dgvr.Cells["Attr"];
                if (dgvc != null && dgvc.Value != null)
                {
                    string term = dgvc.FormattedValue.ToString();
                    if (term.Length > 0 && !iterms.Contains(term))
                    {
                        iterms.Add(term);
                        dgvc.ErrorText = null;
                    }
                    else
                    {
                        dgvc.ErrorText = "The name can not be empty and should not be already existed.";
                        noError = false;
                    }
                }
                dgvc = dgvr.Cells["Restrict"];
                if (dgvc != null && dgvc.Value != null)
                {
                    string cond = dgvc.FormattedValue.ToString();
                    if (cond.Length > 0)
                    {
                        try
                        {
                            DCMEx.DCMEx dcmEx = new DCMEx.DCMEx(cond);
                            dgvc.ErrorText = null;
                        }
                        catch (Exception)
                        {
                            dgvc.ErrorText = "The restrict expression is incorrect.";
                            noError = false;
                        }
                    }
                }
            }
            if (!iterms.Contains(comboBox_keyField.SelectedText))
            {
                MessageBox.Show("Please choose a key name, it's not available.");
                noError = false;
                comboBox_keyField.Focus();
            }
            return noError;
        }

        private void submitSetting()
        {
            this.dataGridView_colCfg.Enabled = false;
            this.comboBox_keyField.Enabled = false;
            ICollection<CustomColDef> ccds = new List<CustomColDef>();
            foreach (DataGridViewRow dgvr in dataGridView_colCfg.Rows)
            {
                CustomColDef ccd = new CustomColDef();
                ccd.Attr=ComponentUtil.DGVUtil.getCellValue(dgvr.Cells["Attr"]);
                ccd.IsRequire = Convert.ToBoolean((dgvr.Cells["Require"] as DataGridViewCheckBoxCell).Value);
                ccd.IsUnique = Convert.ToBoolean((dgvr.Cells["Unique"] as DataGridViewCheckBoxCell).Value);
                ccd.Restrict = ComponentUtil.DGVUtil.getCellValue(dgvr.Cells["Restrict"]);
                ccd.DefaultVal = ComponentUtil.DGVUtil.getCellValue(dgvr.Cells["DefaultVal"]);
                ccd.Alias = ComponentUtil.DGVUtil.getCellValue(dgvr.Cells["Alias"]);
                ccd.IsEnable = Convert.ToBoolean((dgvr.Cells["Enable"] as DataGridViewCheckBoxCell).Value);
                ccds.Add(ccd);
            }

            if (MessageBox.Show(GlobalTextRes.Text("Update local columns' config, the application should restart, are you sure restart now?"),
                    GlobalTextRes.Text("Confirm Message"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (!RunningHandlerNoter.checkForIdle())
                {
                    MessageBox.Show(GlobalTextRes.Text("There are background tasks are executing, the operation in this condition is unsafe, please retry the config later."));
                }
                else
                {
                    CustomColDefGetter.updateCustomColCond(ccds.ToArray());
                    Application.Exit();
                }
            }
        }

    }
}
