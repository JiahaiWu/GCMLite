using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace DCMControlLib.GCM
{
    public class SelectedIndexChangingEventArgs : EventArgs, IDisposable
    {
        #region Instance Member

        private bool cancel = false;
        private int tabPageIndex = -1;
        private TabPageEx tabPage = null;

        #endregion

        #region Constructor

        public SelectedIndexChangingEventArgs(TabPageEx tabPage, int tabPageIndex)
        {
            this.tabPage = tabPage;
            this.tabPageIndex = tabPageIndex;
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets a value indicating whether the event should be canceled.
        /// </summary>
        public bool Cancel
        {
            get { return cancel; }
            set
            {
                if (!value.Equals(cancel))
                    cancel = value;
            }
        }

        /// <summary>
        /// Gets the zero-based index of the TabPageEx in the KRBTabControl.TabPages collection.
        /// </summary>
        public int TabPageIndex
        {
            get { return tabPageIndex; }
        }

        /// <summary>
        /// Gets the TabPageEx the event is occurring for.
        /// </summary>
        public TabPageEx TabPage
        {
            get { return tabPage; }
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
