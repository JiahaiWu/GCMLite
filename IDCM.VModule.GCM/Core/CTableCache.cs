using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using IDCM.Base;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DCMControlLib;
using IDCM.ComponentUtil;

namespace IDCM.Core
{
    public class CTableCache
    {
        #region Constructor&Destructor

        /// <summary>
        /// 本地数据表记录的缓存管理类的构造方法
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="ccds"></param>
        public CTableCache(DataGridView dgv,ICollection<CustomColDef> ccds)
        {
            this.dgv = dgv;
            foreach (CustomColDef ccd in ccds)
            {
                DCMTextDGVColumn dgvc = new DCMTextDGVColumn();
                dgvc.Name = ccd.Attr;
                dgvc.HeaderText = ccd.Alias;
                dgvc.Visible = ccd.IsEnable;
                dgvc.Tag = ccd.IsUnique;
                dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                dgv.Columns.Add(dgvc);
            }
            dgv.AllowUserToOrderColumns = true;
            dgv.Invalidate();
            dgv.Update();

            dataTable = new DataTable();
            foreach (CustomColDef ccd in ccds)
            {
                DataColumn col = new DataColumn();
                col.ColumnName = ccd.Attr;
                col.Caption = ccd.Alias;
                col.DefaultValue = ccd.DefaultVal;
                col.DataType = typeof(string);
                col.AllowDBNull = !ccd.IsRequire;
                dataTable.Columns.Add(col);
            }
            //////////////////////////////////////////
            keyIndexs = new Dictionary<string, int>();
        }
        #endregion

        #region Methods

        internal void updateCustomColCond(int cursor,CustomColDef ccd)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                DCMTextDGVColumn dgvc = dgv.Columns[cursor] as DCMTextDGVColumn;
                if (dgvc != null)
                {
                    CustomColDefGetter.updateCustomColCond(ccd);
                    dgvc.Name = ccd.Attr;
                    dgvc.HeaderText = ccd.Alias;
                    dgvc.Visible = ccd.IsEnable;
                    dgvc.Tag = ccd.IsUnique;
                }
            }));
        }

        internal void addRow(Dictionary<string, string> mapvalues)
        {
            string value = null;
            if (mapvalues.TryGetValue(KeyName, out value))
            {
                int idx = -1;
                if (!keyIndexs.TryGetValue(value, out idx) || idx<0)
                {
                    ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
                    {
                        idx = dgv.Rows.Add();
                        keyIndexs[value] = idx;
                    }));
                }
                ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
                {
                    DataGridViewRow dgvr = dgv.Rows[idx];
                    foreach (KeyValuePair<string, string> kvpair in mapvalues)
                    {
                        dgvr.Cells[kvpair.Key].Value = kvpair.Value;
                    }
                }));
            }
        }


        internal Dictionary<string, int> getIAttrMapping()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();
            DataGridViewColumnCollection dgvcc = dgv.Columns;
            foreach(DataGridViewColumn dgvc in dgvcc)
            {
                if (dgvc.Visible)
                    map[dgvc.Name] = dgvc.Index;
            }
            return map;
        }
        internal int getKeyColIndex()
        {
            DataGridViewColumnCollection dgvcc = dgv.Columns;
            foreach (DataGridViewColumn dgvc in dgvcc)
            {
                if (dgvc.Name.Equals(KeyName))
                   return dgvc.Index;
            }
            return 0;
        }
        internal int getRowCount()
        {
            return this.dgv.RowCount;
        }

        internal Dictionary<int, string> getIRow(int ridx)
        {
            if (ridx > -1 && ridx < dgv.RowCount)
            {
                DataGridViewRow dgvr = dgv.Rows[ridx];
                if (dgvr.IsNewRow)
                    return null;
                else
                {
                    Dictionary<int, string> vals = new Dictionary<int, string>();
                    foreach (DataGridViewCell cell in dgvr.Cells)
                    {
                        if (cell.Visible && cell.Value!=null)
                            vals[cell.ColumnIndex] = cell.FormattedValue.ToString();
                    }
                    return vals;
                }
            }
            return null;
        }
        internal Dictionary<string, string> getRow(int ridx)
        {
            if (ridx > -1 && ridx < dgv.RowCount)
            {
                DataGridViewRow dgvr = dgv.Rows[ridx];
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
        internal DataGridViewRow getDGVRow(int ridx)
        {
            if (ridx > -1 && ridx < dgv.RowCount)
            {
                DataGridViewRow dgvr = dgv.Rows[ridx];
                if (dgvr.IsNewRow)
                    return null;
                return dgvr;
            }
            return null;
        }
        #endregion

        #region Members

        internal string KeyName
        {
            get
            {
                return CustomColDefGetter.KeyName;
            }
        }
        /// <summary>
        /// 整体数据更新共享锁对象
        /// </summary>
        public readonly object GSyncRoot=new object();
        /// <summary>
        /// 主键映射表缓存表设定
        /// </summary>
        private Dictionary<string, int> keyIndexs;
        /// <summary>
        /// 缓存表（暂为备用）
        /// </summary>
        private DataTable dataTable;
        /// <summary>
        /// 数据表视图
        /// </summary>
        private DataGridView dgv;
        #endregion
    }
}
