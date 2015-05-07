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
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = true;
            //this.EnableHeadersVisualStyles = false;
            //this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.EditMode = DataGridViewEditMode.EditOnKeystroke;
            //this.ShowEditingIcon = false;
            //this.Location = new System.Drawing.Point(0, 0);
            //this.Size = new System.Drawing.Size(250, 125);
            
            //used to attach event-handlers to the events of the editing control(nice name!)
            //dgv.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Mydgv_EditingControlShowing);
            // not implemented here, but I still like the name DataGridViewEditingControlShowingEventHandler :o) LOL
            //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;

            this.RowHeadersWidth = 60;
            //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RowHeadersDefaultCellStyle.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RowHeadersDefaultCellStyle.BackColor = Color.LightGray;
            this.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            this.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            this.RowPostPaint += OnRowPostPaint;
            

            this.ColumnAdded += OnColumnsUpdated;
            this.ColumnRemoved += OnColumnsUpdated;
            this.ColumnDisplayIndexChanged += OnColumnsUpdated;
            this.ColumnNameChanged += OnColumnsUpdated;
            this.ColumnHeaderCellChanged += OnColumnsUpdated;
            this.ColumnDataPropertyNameChanged += OnColumnsUpdated;
            this.ColumnStateChanged += OnColumnStateChanged;
            
            
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



        private void OnColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            if (ocMenu != null && !ocMenu.Visible)
                ocMenu.clear();
        }

        private void OnOptionMenuChanged(object sender, DCMControlLib.Pop.MenuItemEventArgs e)
        {
            this.Columns[e.MenuItem.Text].Visible = e.MenuItem.Checked;
        }
        /// <summary>
        /// 设置Datagridview显示编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, this.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                (e.RowIndex + 1).ToString(),
                this.RowHeadersDefaultCellStyle.Font, rectangle,
                this.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        private void OnColumnsUpdated(object sender, DataGridViewColumnEventArgs e)
        {
            if(ocMenu!=null && !ocMenu.Visible)
                ocMenu.clear();
        }
        private void OnCopyClick(object sender, EventArgs e)
        {
            DataObject d = this.GetClipboardContent();
            Clipboard.SetDataObject(d);
        }
        /// <summary>
        /// 从剪贴板粘贴文本型数据记录到目标区域
        /// This will be moved to the util class so it can service any paste into a DGV
        /// </summary>
        private void OnPasteClick(object sender, EventArgs e)
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int iFail = 0, iRow = this.CurrentCell.RowIndex;
                int iCol = this.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                foreach (string line in lines)
                {
                    if (iRow < this.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
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
                    if (iFail > 0)
                        MessageBox.Show(string.Format("{0} updates failed due to read only column setting", iFail));
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
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
                trySwitchNextCell();
            }
        }

        private void trySwitchNextCell()
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
                        this.CurrentCell = oCell;
                        break;
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
                            this.CurrentCell = oCell;
                            break;
                        }
                        else
                            iCol++;
                    }
                }
            }
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
                    ocMenu.Show(this, new Point(e.X, e.Y));
                }
            }
        }
        private void OnCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
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
