﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Base;
using IDCM.Base.Utils;
using IDCM.ComponentUtil;

namespace IDCM.Forms
{
    public partial class AttrMapOptionDlg : Form
    {
        #region Constructor&Destructor
        public AttrMapOptionDlg()
        {
            InitializeComponent();
            this.button_confirm.Text = IDCM.Base.GlobalTextRes.Text("Confirm");
            this.radioButton_custom.Text = IDCM.Base.GlobalTextRes.Text("Custom Mapping");
            this.radioButton_exact.Text = IDCM.Base.GlobalTextRes.Text("Exact Match");
            this.radioButton_similarity.Text = IDCM.Base.GlobalTextRes.Text("Similarity Match");
            this.button_cancel.Text = IDCM.Base.GlobalTextRes.Text("Cancel");
            this.label_designateScope.Text = GlobalTextRes.Text("Designate Scope");
            this.comboBox_designateScope.Text = GlobalTextRes.Text("Default");
            this.Text = IDCM.Base.GlobalTextRes.Text("AttrMappingOptionDlg");

            mapAttrMenu = new DCMControlLib.Pop.SingleOptionalContextMenu();
            mapAttrMenu.OptionMenuChanged += mapAttrMenu_OptionMenuChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 设置初始化映射源和映射目标字符串集合，并指定有效映射返回字典的引用对象
        /// </summary>
        /// <param name="xlscols"></param>
        /// <param name="dbList"></param>
        /// <param name="mapping"></param>
        public void setInitCols(ICollection<string> xlscols, ICollection<string> dbList, ref Dictionary<string, string> mapping)
        {
            this.srcCols = xlscols.ToList();
            this.destCols = dbList.ToList();
            this.mapping = mapping;
            computeSimilarMapping();
            foreach (string dcol in this.destCols)
            {
                mapAttrMenu.addMenu(dcol);
            }
        }
        /// <summary>
        /// 根据字符串源和目标集合，使用编辑距离的计算方法计算相似度，并以一定的阈值通过筛选相似的映射对
        /// </summary>
        /// <param name="threshold"></param>
        public void computeSimilarMapping(double threshold = 0.7)
        {
            Dictionary<ObjectPair<string, string>, double> mappingEntries = new Dictionary<ObjectPair<string, string>, double>();
            List<string> baseList = new List<string>();
            foreach (string str in destCols)
            {
                baseList.Add(str);
            }
            StringSimilarity.computeSimilarMap(srcCols, baseList, ref mappingEntries);
            mapping.Clear();
            foreach (KeyValuePair<ObjectPair<string, string>, double> kvpair in mappingEntries)
            {
                if (kvpair.Value >= threshold)
                {
                    mapping[kvpair.Key.Val] = kvpair.Key.Key;
                }
            }
            foreach (string col in srcCols)
            {
                if (!mapping.ContainsKey(col))
                {
                    mapping[col] = null;
                }
            }
            this.dataGridView_map.Rows.Clear();
            foreach (KeyValuePair<string, string> mappair in mapping)
            {
                this.dataGridView_map.Rows.Add(new string[] { mappair.Key, null, mappair.Value,null, null });
            }
            radioButton_similarity.Checked = true;
        }
        /// <summary>
        /// 更新引用的字典的有效映射的返回映射对
        /// </summary>
        public void setExtractMapping()
        {
            HashSet<string> baseSet = new HashSet<string>(destCols);
            mapping.Clear();
            foreach (string col in srcCols)
            {
                if (baseSet.Contains(col))
                {
                    mapping[col] = col;
                }
                else
                {
                    mapping[col] = null;
                }
            }
            this.dataGridView_map.Rows.Clear();
            foreach (KeyValuePair<string, string> mappair in mapping)
            {
                this.dataGridView_map.Rows.Add(new string[] { mappair.Key, null,mappair.Value, null,null });
            }
            radioButton_exact.Checked = true;
        }
        /// <summary>
        /// 获取未匹配的目标字符串集合
        /// </summary>
        /// <returns></returns>
        private HashSet<string> bindedDestCols()
        {
            HashSet<string> dests = new HashSet<string>();
            foreach (DataGridViewRow dgvr in dataGridView_map.Rows)
            {
                if (dgvr.IsNewRow)
                    continue;
                DataGridViewCell dgvc = dgvr.Cells[2];
                if (dgvc != null)
                {
                    string col = DGVUtil.getCellValue(dgvc);
                    if (col != null && col.Length > 0)
                    {
                        dests.Add(col);
                    }
                }
            }
            return dests;
        }
        #endregion

        #region Events&Handlings

        /// <summary>
        /// 取消操作事件处理并关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            mapping.Clear();
            this.Close();
        }
        /// <summary>
        /// 确认映射对配置并关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 启用模糊匹配模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_similarity_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_similarity.Checked)
                computeSimilarMapping();
        }
        /// <summary>
        /// 启用精确匹配模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_exact_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_exact.Checked)
                setExtractMapping();
        }

        /// <summary>
        /// 自动行号实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_map_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_map.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridView_map.RowHeadersDefaultCellStyle.Font, rectangle,
                dataGridView_map.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            dataGridView_map.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }
        /// <summary>
        /// 映射编辑事件处理入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_map_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;
                if (dataGridView_map.Columns[e.ColumnIndex].HeaderText.Equals("Unbind"))
                {
                    DataGridViewCell dgvcell = dataGridView_map.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvcell.Value = null;
                    string col = dataGridView_map.Rows[e.RowIndex].Cells[0].Value.ToString();
                    mapping[col] = null;
                    dataGridView_map.Rows[e.RowIndex].Cells[2].Value = null;
                    radioButton_custom.Checked = true;
                }
                else if (dataGridView_map.Columns[e.ColumnIndex].HeaderText.Equals("Rebind"))
                {
                    mapAttrMenu.checkAndResetOthers(DGVUtil.getCellValue(dataGridView_map.Rows[e.RowIndex].Cells[2]));
                    mapAttrMenu.Tag = e.RowIndex;
                    Point plocation = this.PointToScreen(this.Location);
                    mapAttrMenu.Show(this, new Point(MousePosition.X - plocation.X, MousePosition.Y - plocation.Y));
                }
            }
        }
        private void mapAttrMenu_OptionMenuChanged(object sender, DCMControlLib.Pop.MenuItemEventArgs e)
        {
            if (mapAttrMenu != null && mapAttrMenu.Tag != null)
            {
                int rowIndex = Convert.ToInt32(mapAttrMenu.Tag);
                if (rowIndex > -1 && rowIndex < dataGridView_map.RowCount)
                {
                    if (e.MenuItem.Checked)
                    {
                        dataGridView_map.Rows[rowIndex].Cells[2].Value = e.MenuItem.Text;
                        mapping[dataGridView_map.Rows[rowIndex].Cells[0].FormattedValue.ToString()] = e.MenuItem.Text;
                    }
                    else
                    {
                        dataGridView_map.Rows[rowIndex].Cells[2].Value = null;
                        mapping.Remove(dataGridView_map.Rows[rowIndex].Cells[0].FormattedValue.ToString());
                    }
                    radioButton_custom.Checked = true;
                }
                mapAttrMenu.Tag = null;
            }
        }

        #endregion

        #region Members

        private List<string> srcCols = null;
        private List<string> destCols = null;
        private Dictionary<string, string> mapping = null;
        private DCMControlLib.Pop.SingleOptionalContextMenu mapAttrMenu = null;
        #endregion
    }
}
