using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace DCMControlLib.Pop
{
    class OptionalContextMenu:ContextMenu
    {
        public OptionalContextMenu()
            : base()
        {
            this.Popup += OnPopUpNote;
            this.Collapse += OnCollapseNote;
        }

        private void OnCollapseNote(object sender, EventArgs e)
        {
            this.isShown = false;
        }

        private void OnPopUpNote(object sender, EventArgs e)
        {
            this.isShown = true;
        }
        public OptionalContextMenu(Dictionary<string,bool> nameMaps)
            : base()
        {
            if (nameMaps != null)
            {
                foreach (KeyValuePair<string, bool> pair in nameMaps)
                {
                    if (pair.Key != null)
                    {
                        MenuItem mi = new MenuItem(pair.Key, OnContextMenuClick);
                        mi.Checked = pair.Value;
                        this.MenuItems.Add(mi);
                    }
                }
            }
        }
        private void OnContextMenuClick(object sender, EventArgs e)
        {
            if (sender != null)
            {
                MenuItem mi = sender as MenuItem;
                mi.Checked = !mi.Checked;
                if(OptionMenuChanged!=null)
                    OptionMenuChanged(mi, new MenuItemEventArgs(mi));
            }
        }
        public void addMenu(string name,bool isChoosed)
        {
            MenuItem mi = new MenuItem(name, OnContextMenuClick);
            mi.Checked = isChoosed;
            this.MenuItems.Add(mi);
        }
        public Dictionary<string, bool> getOptionNameMaps()
        {
            Dictionary<string, bool> nameMaps = new Dictionary<string, bool>();
            foreach (MenuItem mi in this.MenuItems)
            {
                nameMaps[mi.Text] = mi.Checked;
            }
            return nameMaps;
        }

        internal void clear()
        {
            this.MenuItems.Clear();
        }

        [Description("Get the visible status.")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool Visible { 
            get
            {
                return isShown;
            } 
        }
        
        public int Count { 
            get{
                return MenuItems.Count;
            }
        }
        public delegate void MenuItemEventHandler(object sender, MenuItemEventArgs e);
        public event MenuItemEventHandler OptionMenuChanged;
        private volatile bool isShown = false;

    }
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
