using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DCMControlLib.Pop
{
    public class MenuItemEventArgs : EventArgs
    {
        public MenuItemEventArgs()
        {
        }
        public MenuItemEventArgs(MenuItem mi)
        {
            item = mi;
        }
        public MenuItem MenuItem
        {
            get
            {
                return item;
            }
        }
        private MenuItem item;
    }
}
