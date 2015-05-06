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
            this.KeyDown += OnPasteDetect;
            this.ColumnAdded += OnColumnsUpdated;
            this.ColumnRemoved += OnColumnsUpdated;
            this.ColumnDisplayIndexChanged += OnColumnsUpdated;
            this.ColumnNameChanged += OnColumnsUpdated;
            this.ColumnHeaderCellChanged += OnColumnsUpdated;
            this.ColumnDataPropertyNameChanged += OnColumnsUpdated;
            this.ColumnHeaderMouseClick += OnColumnHeaderMouseClick;
            this._customHeaderView = true;
            this.ocMenu = new Pop.OptionalContextMenu();
            ocMenu.OptionMenuChanged += OnOptionMenuChanged;
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
        /// <summary>
        /// 绑定剪贴板复制Ctrl+C、行插入 Ctrl+Insert 等快捷键处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPasteDetect(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = this.GetClipboardContent();
                Clipboard.SetDataObject(d);
            }
            if (e.Control && e.KeyCode == Keys.Insert)
            {
                int iRow=0;
                if(this.CurrentCell!=null)
                {
                    iRow= this.CurrentCell.RowIndex;
                    if (iRow < 0)
                        iRow = 0;
                    if (iRow < this.RowCount)
                        iRow = this.RowCount;
                }else{
                    iRow=this.RowCount;
                }
                this.Rows.Insert(iRow, 1);
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
        private volatile bool _customHeaderView = true;
        private Pop.OptionalContextMenu ocMenu = null;
    }
}
