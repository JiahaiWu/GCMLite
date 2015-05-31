using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace DCMControlLib.Pop
{
    public class SingleOptionalContextMenu:ContextMenu
    {
        public SingleOptionalContextMenu()
            : base()
        {
            this.Popup += OnPopUpNote;
            this.Collapse += OnCollapseNote;
        }

        public SingleOptionalContextMenu(Dictionary<string, bool> nameMaps)
            : base()
        {
            if (nameMaps != null)
            {
                foreach (KeyValuePair<string, bool> pair in nameMaps)
                {
                    if (pair.Key != null)
                    {
                        MenuItem mi = new MenuItem(pair.Key, OnContextMenuClick);
                        mi.RadioCheck =true;
                        mi.Checked = pair.Value;
                        if (pair.Value)
                            checkAndResetOthers(mi);
                        this.MenuItems.Add(mi);
                    }
                }
            }
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
        private void OnContextMenuClick(object sender, EventArgs e)
        {
            if (sender != null)
            {
                MenuItem mi = sender as MenuItem;
                mi.Checked = !mi.Checked;
                if (mi.Checked)
                {
                    checkAndResetOthers(mi);
                }
                if(OptionMenuChanged!=null)
                    OptionMenuChanged(mi, new MenuItemEventArgs(mi));
            }
        }
        public void addMenu(string name,bool isChoosed=false)
        {
            MenuItem mi = new MenuItem(name, OnContextMenuClick);
            mi.RadioCheck = true;
            mi.Checked = isChoosed;
            if (mi.Checked)
            {
                checkAndResetOthers(mi);
            }
            this.MenuItems.Add(mi);
        }
        public string getSelectedNameMap()
        {
            foreach (MenuItem mi in this.MenuItems)
            {
                if (mi.Checked)
                    return mi.Text;
            }
            return null;
        }

        private void checkAndResetOthers(MenuItem specificItem)
        {
            foreach (MenuItem mi in this.MenuItems)
            {
                if (mi != specificItem)
                {
                    mi.Checked = false;
                }
                else
                {
                    mi.Checked = true;
                }
            }
        }
        public void checkAndResetOthers(string specificItem)
        {
            foreach (MenuItem mi in this.MenuItems)
            {
                if (!mi.Text.Equals(specificItem))
                {
                    mi.Checked = false;
                }
                else
                {
                    mi.Checked = true;
                }
            }
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
    
}
