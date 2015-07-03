using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace DCMControlLib
{
    public class DCMDataGridView:DataGridView
    {
        public DCMDataGridView()
            : base()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////
            this.BackgroundColor = Color.FromArgb(255, 255, 255);
            this.GridColor = Color.FromArgb(219, 217, 218);
            this.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            //列Header的背景色
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225,235,247);
            //奇数行的背景色
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 246, 244);

            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            this.RowPostPaint += OnRowPostPaint;
            
            this.ColumnAdded += OnColumnsUpdated;
            this.ColumnRemoved += OnColumnsUpdated;
            this.ColumnDisplayIndexChanged += OnColumnsUpdated;
            this.ColumnNameChanged += OnColumnsUpdated;
            this.ColumnHeaderCellChanged += OnColumnsUpdated;
            this.ColumnDataPropertyNameChanged += OnColumnsUpdated;
            this.ColumnStateChanged += OnColumnStateChanged;
            this.CellPainting+=DCMDataGridView_CellPainting;
            
            this._customHeaderView = true;
            this.ocMenu = new Pop.OptionalContextMenu();
            this.ColumnHeaderMouseClick += OnColumnHeaderMouseClick;
            ocMenu.OptionMenuChanged += OnOptionMenuChanged;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            this._pasteAble = false;
            this.cellCopyMenu = new ContextMenu();
            this.cellCopyMenu.MenuItems.Add(new MenuItem("Copy", OnCopyClick));
            this.cellCopyMenu.MenuItems.Add(new MenuItem("Paste", OnPasteClick));
            this.KeyDown += OnKeyDownDetect;
            this.CellMouseClick += OnCellMouseClick;
        }

        private void OnOptionMenuChanged(object sender, DCMControlLib.Pop.MenuItemEventArgs e)
        {
            if (this.DisplayedColumnCount(true) < 2 && e.MenuItem.Checked==false)
            {
                MessageBox.Show("Hide all columns is improper.");
                e.MenuItem.Checked = true;
            }
            else
            {
                this.Columns[e.MenuItem.Text].Visible = e.MenuItem.Checked;
            }
        }
        /// <summary>
        /// 设置Datagridview显示编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, this.RowHeadersWidth - 4, e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    this.RowHeadersDefaultCellStyle.Font, rectangle,
                    this.RowHeadersDefaultCellStyle.ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            }
            
        }


        private void DCMDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            /////////////////////////////////////////////////////////////////////////////////////
            //对表头部分自定义排序标志绘图实现
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(e.CellBounds.Location.X + e.CellBounds.Width - e.CellBounds.Height + 2,
                    e.CellBounds.Location.Y + 2, e.CellBounds.Height - 4, e.CellBounds.Height - 4);
                if (this.SortedColumn != null && this.SortedColumn.Index.Equals(e.ColumnIndex))
                {
                    SolidBrush brush = new SolidBrush(e.CellStyle.BackColor);
                    e.Graphics.FillRectangle(brush, rect);
                    if (SortOrder.Descending.Equals(this.SortOrder))
                    {
                        e.Graphics.DrawImage(global::DCMControlLib.Properties.Resources.des, rect);
                    }
                    else if (SortOrder.Ascending.Equals(this.SortOrder))
                    {
                        e.Graphics.DrawImage(global::DCMControlLib.Properties.Resources.asc, rect);
                    }
                    e.PaintContent(e.CellBounds);
                    e.Handled = true;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////
        }

        private void OnColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            if (ocMenu != null && !ocMenu.Visible)
                ocMenu.clear();
        }
        private void OnColumnsUpdated(object sender, DataGridViewColumnEventArgs e)
        {
            if(ocMenu!=null && !ocMenu.Visible)
                ocMenu.clear();
            if (e.Column != null)
            {
                SizeF size = TextRenderer.MeasureText(e.Column.HeaderText, e.Column.DefaultCellStyle.Font);
                if (size.Width+32 > e.Column.Width)
                    e.Column.Width =size.Width<200?(int)Math.Ceiling(size.Width+e.Column.DefaultCellStyle.Padding.Horizontal+32):232;
            }
        }
        private void OnCopyClick(object sender, EventArgs e)
        {
            DataObject d = this.GetClipboardContent();
            if (d == null)
                return;
            Clipboard.SetDataObject(d);
        }
        /// <summary>
        /// 从剪贴板粘贴文本型数据记录到目标区域
        /// This will be moved to the util class so it can service any paste into a DGV
        /// </summary>
        private void OnPasteClick(object sender, EventArgs e)
        {
            if (_pasteAble)
            {
                try
                {
                    string s = Clipboard.GetText();
                    if (s == null || this.CurrentCell == null)
                        return;
                    string[] lines = s.Split('\n');
                    int iFail = 0, iRow = this.CurrentCell.RowIndex;
                    int iCol = this.CurrentCell.ColumnIndex;
                    DataGridViewCell oCell;
                    foreach (string line in lines)
                    {
                        if (iRow < this.RowCount && line.Length > 0)
                        {
                            string[] sCells = line.Split('\t');
                            for (int i = 0; i < sCells.GetLength(0); i++)
                            {
                                if (iCol + i < this.ColumnCount)
                                {
                                    oCell = this[iCol + i, iRow];
                                    if (!oCell.ReadOnly)
                                    {
                                        if (oCell.Value == null || oCell.Value.ToString() != sCells[i])
                                        {
                                            oCell.Value = Convert.ChangeType(sCells[i], oCell.ValueType);
                                            oCell.Style.BackColor = Color.Tomato;
                                        }
                                        else
                                            iFail++;//only traps a fail if the data has changed and you are pasting into a read only cell
                                    }
                                }
                                else
                                { break; }
                            }
                            iRow++;
                        }
                        else
                        { break; }
                        //////////////////////////////////////////////////////////////////
                        //if (iFail > 0)
                        //    MessageBox.Show(string.Format("{0} updates failed due to read only column setting", iFail));
                        //@Deprecated
                        ///////////////////////////////////////////////////////////////////
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("The data you pasted is in the wrong format for the cell");
                    return;
                }
            }
        }

        /// <summary>
        /// 绑定剪贴板复制Ctrl+C、行插入 Ctrl+Insert 等快捷键处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownDetect(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                OnCopyClick(sender, e);
            }
            if (_pasteAble)
            {
                if (e.Control && e.KeyCode == Keys.V)
                {
                    OnPasteClick(sender, e);
                }
            }
            if(e.KeyCode==Keys.Tab)
            {
                DataGridViewCell oCell = tryGetNextCell();
                if(oCell!=null)
                    this.CurrentCell = oCell;
            }
        }

        protected DataGridViewCell tryGetNextCell()
        {
            if (this.CurrentCell != null)
            {
                int iRow = this.CurrentCell.RowIndex;
                int iCol = this.CurrentCell.ColumnIndex;
                while (iCol + 1 < this.ColumnCount)
                {
                    DataGridViewCell oCell = this[iCol + 1, iRow];
                    if (oCell.Visible)
                    {
                        if (this.IsCurrentCellInEditMode)
                        {
                            this.EndEdit();
                        }
                        return oCell;
                    }
                    else
                        iCol++;
                }
                if (iCol + 1 >= this.ColumnCount && iRow < this.RowCount)
                {
                    iRow++;
                    iCol = 0;
                    while (iCol < this.ColumnCount)
                    {
                        DataGridViewCell oCell = this[iCol, iRow];
                        if (oCell.Visible)
                        {
                            if (this.IsCurrentCellInEditMode)
                            {
                                this.EndEdit();
                            }
                            return oCell;
                        }
                        else
                            iCol++;
                    }
                }
            }
            return null;
        }
        private void OnColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_customHeaderView && ocMenu!=null)
                {
                    if (ocMenu.Count < 1)
                    {
                        foreach (DataGridViewColumn dgvc in this.Columns)
                        {
                            ocMenu.addMenu(dgvc.HeaderText, dgvc.Visible);
                        }
                    }
                    Point plocation = this.PointToScreen(this.Location);
                    ocMenu.Show(this, new Point(MousePosition.X - plocation.X, MousePosition.Y - plocation.Y));
                }
            }
        }
        private void OnCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.ColumnIndex>-1 && e.RowIndex>-1)
                this.CurrentCell=this[e.ColumnIndex,e.RowIndex];
            if (e.Button == MouseButtons.Right)
            {
                if (_pasteAble && cellCopyMenu!=null)
                {
                    cellCopyMenu.Show(this, new Point(e.X, e.Y));
                }
            }
        }

        [Description("Determines whether the DataGridView header visible can be customs by pop-up context menu.")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool IsCustomHeaderView
        {
            get { return _customHeaderView; }
            set
            {
                if (!value.Equals(_customHeaderView))
                {
                    _customHeaderView = value;
                }
            }
        }

        [Description("Determines whether the DataGridView Cell Copy and paste supported by inner pop-up context menu.")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool IsDefaultPasteAble
        {
            get { return _pasteAble; }
            set
            {
                if (!value.Equals(_pasteAble))
                {
                    _pasteAble = value;
                }
            }
        }
        private volatile bool _customHeaderView = true;
        private volatile bool _pasteAble = true;
        private Pop.OptionalContextMenu ocMenu = null;
        private ContextMenu cellCopyMenu = null;
    }
}
