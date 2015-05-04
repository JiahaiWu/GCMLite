using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace DCMControlLib.GCM
{
    public class ContextMenuShownEventArgs : EventArgs, IDisposable
    {
        #region Instance Member

        private ContextMenuStrip contextMenu = null;
        private Point menuLocation = Point.Empty;

        #endregion

        #region Constructor

        public ContextMenuShownEventArgs(ContextMenuStrip contextMenu, Point menuLocation)
        {
            this.contextMenu = contextMenu;
            this.menuLocation = menuLocation;
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets the drop-down menu of the control.It shows when a user clicks the drop-down icon on the caption.
        /// </summary>
        public ContextMenuStrip ContextMenu
        {
            get { return contextMenu; }
        }

        /// <summary>
        /// Gets or sets the drop-down menu location on the screen coordinates.
        /// </summary>
        public Point MenuLocation
        {
            get { return menuLocation; }
            set
            {
                menuLocation = value;
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
