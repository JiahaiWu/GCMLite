using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using IDCM.Base;
using IDCM.DataTransfer;
using IDCM.Base.ComPO;
using IDCM.MsgDriver;
using IDCM.ComponentUtil;

namespace IDCM.Core
{
    internal class GCMTableCache
    {
        public GCMTableCache(TextBox textBox_ccinfoId, TextBox textBox_pwd, CheckBox checkBox_remember, DCMControlLib.DCMDataGridView dgv_overview, DCMControlLib.Tree.DCMTreeView tree_detail)
        {
            this.textBox_ccinfoId = textBox_ccinfoId;
            this.textBox_pwd = textBox_pwd;
            this.checkBox_remember = checkBox_remember;
            this.dgv_overview = dgv_overview;
            this.tree_detail = tree_detail;
            keyIndexs = new Dictionary<string, int>();
            keyName = "id";
        }
        internal string UserName
        {
            get{
                return this.textBox_ccinfoId.Text.Trim();
            }
            set
            {
                this.textBox_ccinfoId.Text = value;
            }
        }
        internal string Password
        {
            get
            {
                return this.textBox_pwd.Text.Trim();
            }
            set
            {
                this.textBox_pwd.Text = value;
            }
        }
        internal bool RememberLogin
        {
            get
            {
                return this.checkBox_remember.Checked;
            }
            set
            {
                this.checkBox_remember.Checked = value;
            }
        }
        internal void addOverViewRow(Dictionary<string, string> valMap)
        {
            if (!dgv_overview.Columns.Contains(keyName))
            {
                DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
                dgvtbc.Name = keyName;
                dgvtbc.HeaderText = keyName;
                dgvtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                ControlAsyncUtil.SyncInvoke(dgv_overview, new ControlAsyncUtil.InvokeHandler(delegate()
                {
                    dgv_overview.Columns.Add(dgvtbc);
                }));
            }
            //add valMap note Tag into loadedNoter Map
            int dgvrIdx = -1;
            if (!keyIndexs.TryGetValue(valMap[keyName], out dgvrIdx))
            {
                dgvrIdx = dgv_overview.RowCount;
            }
            DataGridViewRow dgvr = new DataGridViewRow();
            dgvr.CreateCells(dgv_overview);
            ControlAsyncUtil.SyncInvoke(dgv_overview, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                dgv_overview.Rows.InsertRange(dgvrIdx, dgvr);
                keyIndexs[valMap[keyName]]= dgvrIdx;
                foreach (KeyValuePair<string, string> entry in valMap)
                {
                    //if itemDGV not contains Column of entry.key
                    //   add Column named with entry.key
                    //then merge data into itemDGV View.
                    //(if this valMap has exist in loadedNoter Map use Update Method else is append Method.) 
                    if (!dgv_overview.Columns.Contains(entry.Key))
                    {
                        DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
                        dgvtbc.Name = entry.Key;
                        dgvtbc.HeaderText = entry.Key;
                        dgvtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgv_overview.Columns.Add(dgvtbc);
                    }
                    DataGridViewCell dgvc = dgv_overview.Rows[dgvrIdx].Cells[entry.Key];
                    dgvc.Value = entry.Value;
                }
            }));
        }
        internal string getSIDByRowIdx(int ridx)
        {
            if (ridx > -1 && ridx < dgv_overview.RowCount)
            {
                DataGridViewRow dgvr = dgv_overview.Rows[ridx];
                if (dgvr.IsNewRow)
                    return null;
                else
                {
                    object val = dgvr.Cells[keyName].FormattedValue;
                    return val == null ? null : val.ToString();
                }
            }
            return null;
        }
        internal bool overViewRowIndexValid(int rowIndex)
        {
            if (rowIndex < 0)
                return false;
            return (rowIndex < dgv_overview.RowCount && !dgv_overview.Rows[rowIndex].IsNewRow);
        }

        internal bool overViewFocusRecently(int rowIndex)
        {
            string sid = getSIDByRowIdx(rowIndex);
            if (sid != null && sid.Equals(lastTreeViewSID))
            {
                long elapsedTicks = DateTime.Now.Ticks - lastTreeViewTimeStamp;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                return elapsedSpan.TotalMilliseconds < 2000;
            }
            return false;
        }
        internal int getOverViewRowCount()
        {
            return this.dgv_overview.RowCount;
        }

        internal Dictionary<int, string> getOverViewIRow(int ridx)
        {
            if (ridx > -1 && ridx < dgv_overview.RowCount)
            {
                DataGridViewRow dgvr = dgv_overview.Rows[ridx];
                if (dgvr.IsNewRow)
                    return null;
                else
                {
                    Dictionary<int, string> vals = new Dictionary<int, string>();
                    foreach (DataGridViewCell cell in dgvr.Cells)
                    {
                        if (cell.Visible && cell.Value != null)
                            vals[cell.ColumnIndex] = cell.FormattedValue.ToString();
                    }
                    return vals;
                }
            }
            return null;
        }
        internal Dictionary<string, string> getOverViewRow(int ridx)
        {
            if (ridx > -1 && ridx < dgv_overview.RowCount)
            {
                DataGridViewRow dgvr = dgv_overview.Rows[ridx];
                if (dgvr.IsNewRow)
                    return null;
                else
                {
                    Dictionary<string, string> vals = new Dictionary<string, string>();
                    foreach (DataGridViewCell cell in dgvr.Cells)
                    {
                        if (cell.Visible && cell.Value != null)
                            vals[cell.OwningColumn.Name] = cell.FormattedValue.ToString();
                    }
                    return vals;
                }
            }
            return null;
        }

        internal void resetTree(ComPO.StrainView sv,string sid)
        {
            lastTreeViewSID=sid;
            lastTreeViewTimeStamp = DateTime.Now.Ticks;
            ControlAsyncUtil.SyncInvoke(tree_detail, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                tree_detail.Nodes.Clear();
                if (sv != null)
                {
                    foreach (KeyValuePair<string, object> svEntry in sv.ToDictionary())
                    {
                        TreeNode node = new TreeNode(svEntry.Key,0,0);
                        node.Name = svEntry.Key;
                        if (svEntry.Value is string)
                        {
                            TreeNode subNode = new TreeNode(Convert.ToString(svEntry.Value), 1, 1);
                            subNode.Name = svEntry.Key;
                            node.Nodes.Add(subNode);
                        }
                        else if (svEntry.Value is Dictionary<string, dynamic>)
                        {
                            foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                            {
                                TreeNode subNode = new TreeNode(subEntry.Key,0,0);
                                subNode.Name = subEntry.Key;
                                node.Nodes.Add(subNode);
                                if(subEntry.Value!=null)
                                {
                                    TreeNode subsubNode = new TreeNode(Convert.ToString(subEntry.Value),1,1);
                                    subsubNode.Name = Convert.ToString(subEntry.Value);
                                    subNode.Nodes.Add(subsubNode);
                                }
                            }
                        }
                        tree_detail.Nodes.Add(node);
                    }
                }
            }));
        }

        /// <summary>
        /// 整体数据更新共享锁对象
        /// </summary>
        public readonly object GSyncRoot = new object();
        /// <summary>
        /// 主键名称标记
        /// </summary>
        private string keyName = "id";
        /// <summary>
        /// 主键映射表缓存表设定
        /// </summary>
        private Dictionary<string, int> keyIndexs;
        /// <summary>
        /// 缓存表（暂为备用）
        /// </summary>
        private DataTable dataTable;
        private volatile string lastTreeViewSID=null;
        private long lastTreeViewTimeStamp = 0L;

        /// <summary>
        /// 数据表视图
        /// </summary>
        private DCMControlLib.DCMDataGridView dgv_overview;
        private DCMControlLib.Tree.DCMTreeView tree_detail;
        private TextBox textBox_ccinfoId;
        private TextBox textBox_pwd;
        private CheckBox checkBox_remember;
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

    }
}
