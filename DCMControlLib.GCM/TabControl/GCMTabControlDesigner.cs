using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using DCMControlLib.GCM.Win32;

namespace DCMControlLib.GCM
{
    public class GCMTabControlDesigner : System.Windows.Forms.Design.ParentControlDesigner
    {
        #region Instance Members

        private DesignerVerbCollection _verbs;
        private DesignerActionListCollection _actionLists;

        private IDesignerHost _designerHost;
        private IComponentChangeService _changeService;

        #endregion

        #region Constructor

        public GCMTabControlDesigner()
            : base() { }

        #endregion

        #region Destructor

        ~GCMTabControlDesigner()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Property

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_actionLists == null)
                {
                    _actionLists = new DesignerActionListCollection();
                    _actionLists.Add(new GCMTabControlActionList((GCMTabControl)Control));
                }

                return _actionLists;
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_verbs == null)
                {
                    DesignerVerb[] addVerbs = new DesignerVerb[]
                    {
                        new DesignerVerb("Add Tab", new EventHandler(OnAddTab)),
                        new DesignerVerb("Remove Tab", new EventHandler(OnRemoveTab))
                    };

                    _verbs = new DesignerVerbCollection();
                    _verbs.AddRange(addVerbs);

                    GCMTabControl parentControl = Control as GCMTabControl;
                    if (parentControl != null)
                    {
                        switch (parentControl.TabPages.Count)
                        {
                            case 0:
                                _verbs[1].Enabled = false;
                                break;
                            default:
                                _verbs[1].Enabled = true;
                                break;
                        }
                    }
                }

                return _verbs;
            }
        }

        #endregion

        #region Override Methods

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            _designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
            _changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // Update your designer verb whenever ComponentChanged event occurs.
            if (_changeService != null)
                _changeService.ComponentChanged += new ComponentChangedEventHandler(OnComponentChanged);
        }

        /*  As a general rule, always call the base method first in the PreFilterXxx() methods and last in the 
            PostFilterXxx() methods. This way, all designer classes are given the proper opportunity to apply their 
            changes. The ControlDesigner and ComponentDesigner use these methods to add properties like Visible, 
            Enabled, Name, and Locked. */

        /// <summary>
        /// Override this method to remove unused or inappropriate events.
        /// </summary>
        /// <param name="events">Events collection of the control.</param>
        protected override void PostFilterEvents(System.Collections.IDictionary events)
        {
            events.Remove("StyleChanged");
            events.Remove("MarginChanged");
            events.Remove("PaddingChanged");
            events.Remove("EnabledChanged");
            events.Remove("ImeModeChanged");
            events.Remove("LocationChanged");
            events.Remove("RightToLeftChanged");
            events.Remove("BindingContextChanged");
            events.Remove("RightToLeftLayoutChanged");

            base.PostFilterEvents(events);
        }

        /// <summary>
        /// Override this method to add some properties to the control or change the properties attributes for a dynamic user interface.
        /// </summary>
        /// <param name="properties">Properties collection of the control before than add a new property to the collection by user.</param>
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            // We don't want to show the "Location" and "ShowToolTips" properties for our control at the design-time.
            properties["Location"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
              (PropertyDescriptor)properties["Location"], BrowsableAttribute.No);
            properties["ShowToolTips"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
              (PropertyDescriptor)properties["ShowToolTips"], BrowsableAttribute.No);

            /* After than, we don't want to see some properties at design-time for general reasons(Dynamic property attributes). */
            GCMTabControl parentControl = Control as GCMTabControl;

            if (parentControl != null)
            {
                if (parentControl.HeaderVisibility)
                {
                    properties["ItemSize"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["ItemSize"], BrowsableAttribute.No);
                    properties["TabStyles"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["TabStyles"], BrowsableAttribute.No);
                    properties["Alignments"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["Alignments"], BrowsableAttribute.No);
                    properties["UpDownStyle"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["UpDownStyle"], BrowsableAttribute.No);
                    properties["HeaderStyle"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["HeaderStyle"], BrowsableAttribute.No);
                    properties["TabGradient"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["TabGradient"], BrowsableAttribute.No);
                    properties["IsDrawHeader"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["IsDrawHeader"], BrowsableAttribute.No);
                    properties["CaptionButtons"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["CaptionButtons"], BrowsableAttribute.No);
                    properties["TabBorderColor"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["TabBorderColor"], BrowsableAttribute.No);
                    properties["GradientCaption"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["GradientCaption"], BrowsableAttribute.No);
                    properties["BackgroundColor"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["BackgroundColor"], BrowsableAttribute.No);
                    properties["BackgroundImage"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["BackgroundImage"], BrowsableAttribute.No);
                    properties["BackgroundHatcher"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["BackgroundHatcher"], BrowsableAttribute.No);
                    properties["CaptionRandomizer"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["CaptionRandomizer"], BrowsableAttribute.No);
                    properties["IsDrawTabSeparator"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["IsDrawTabSeparator"], BrowsableAttribute.No);

                    return;
                }

                if (!parentControl.IsCaptionVisible)
                {
                    properties["CaptionButtons"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                        (PropertyDescriptor)properties["CaptionButtons"], BrowsableAttribute.No);
                    properties["CaptionRandomizer"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                        (PropertyDescriptor)properties["CaptionRandomizer"], BrowsableAttribute.No);
                    properties["GradientCaption"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                        (PropertyDescriptor)properties["GradientCaption"], BrowsableAttribute.No);
                }

                if (parentControl.IsDrawHeader)
                {
                    switch (parentControl.HeaderStyle)
                    {
                        case GCMTabControl.TabHeaderStyle.Hatch:
                            properties["BackgroundColor"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                              (PropertyDescriptor)properties["BackgroundColor"], BrowsableAttribute.No);
                            properties["BackgroundImage"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                              (PropertyDescriptor)properties["BackgroundImage"], BrowsableAttribute.No);
                            break;
                        case GCMTabControl.TabHeaderStyle.Solid:
                            properties["BackgroundImage"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                              (PropertyDescriptor)properties["BackgroundImage"], BrowsableAttribute.No);
                            properties["BackgroundHatcher"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                              (PropertyDescriptor)properties["BackgroundHatcher"], BrowsableAttribute.No);
                            break;
                        default:
                            properties["BackgroundColor"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                              (PropertyDescriptor)properties["BackgroundColor"], BrowsableAttribute.No);
                            properties["BackgroundHatcher"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                              (PropertyDescriptor)properties["BackgroundHatcher"], BrowsableAttribute.No);
                            break;
                    }
                }
                else
                {
                    properties["HeaderStyle"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["HeaderStyle"], BrowsableAttribute.No);
                    properties["BackgroundColor"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["BackgroundColor"], BrowsableAttribute.No);
                    properties["BackgroundImage"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["BackgroundImage"], BrowsableAttribute.No);
                    properties["BackgroundHatcher"] = TypeDescriptor.CreateProperty(typeof(GCMTabControl),
                      (PropertyDescriptor)properties["BackgroundHatcher"], BrowsableAttribute.No);
                }
            }
        }

        /// <summary>
        /// Override this method to remove unused or inappropriate properties.
        /// </summary>
        /// <param name="properties">Properties collection of the control.</param>
        protected override void PostFilterProperties(System.Collections.IDictionary properties)
        {
            properties.Remove("Margin");
            properties.Remove("ImeMode");
            properties.Remove("Padding");
            properties.Remove("Enabled");
            properties.Remove("RightToLeft");
            properties.Remove("RightToLeftLayout");
            properties.Remove("ApplicationSettings");
            properties.Remove("DataBindings");

            base.PostFilterProperties(properties);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == (int)User32.Msgs.WM_NCHITTEST)
            {
                if (m.Result.ToInt32() == User32._HT_TRANSPARENT)
                    m.Result = (IntPtr)User32._HT_CLIENT;
            }
        }

        protected override bool GetHitTest(Point point)
        {
            ISelectionService _selectionService = (ISelectionService)GetService(typeof(ISelectionService));
            if (_selectionService != null)
            {
                object selectedObject = _selectionService.PrimarySelection;
                if (selectedObject != null && selectedObject.Equals(this.Control))
                {
                    Point p = this.Control.PointToClient(point);

                    User32.TCHITTESTINFO hti = new User32.TCHITTESTINFO(p, User32.TabControlHitTest.TCHT_ONITEM);

                    Message m = new Message();
                    m.HWnd = this.Control.Handle;
                    m.Msg = User32._TCM_HITTEST;

                    IntPtr lParam = System.Runtime.InteropServices.Marshal.AllocHGlobal(System.Runtime.InteropServices.Marshal.SizeOf(hti));
                    System.Runtime.InteropServices.Marshal.StructureToPtr(hti, lParam, false);
                    m.LParam = lParam;

                    base.WndProc(ref m);
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(lParam);

                    if (m.Result.ToInt32() != -1)
                        return hti.flags != User32.TabControlHitTest.TCHT_NOWHERE;
                }
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _changeService != null)
                _changeService.ComponentChanged -= OnComponentChanged;

            base.Dispose(disposing);
        }

        #endregion

        #region Helper Methods

        /*  When the designer modifies the KRBTabControl.TabPages collection, 
                the Properties window is not updated until the control is deselected and then reselected. To 
                correct this defect, you need to explicitly notify the IDE that a change has been made by using 
                the PropertyDescriptor for the property. */

        private void OnAddTab(Object sender, EventArgs e)
        {
            GCMTabControl parentControl = Control as GCMTabControl;

            TabControl.TabPageCollection oldTabs = parentControl.TabPages;

            // Notify the IDE that the TabPages collection property of the current tab control has changed.
            RaiseComponentChanging(TypeDescriptor.GetProperties(parentControl)["TabPages"]);
            TabPageEx newTab = (TabPageEx)_designerHost.CreateComponent(typeof(TabPageEx));
            newTab.Text = newTab.Name;
            parentControl.TabPages.Add(newTab);
            parentControl.SelectedTab = newTab;
            RaiseComponentChanged(TypeDescriptor.GetProperties(parentControl)["TabPages"], oldTabs, parentControl.TabPages);
        }

        private void OnRemoveTab(Object sender, EventArgs e)
        {
            GCMTabControl parentControl = Control as GCMTabControl;

            if (parentControl.SelectedIndex < 0)
                return;

            TabControl.TabPageCollection oldTabs = parentControl.TabPages;

            // Notify the IDE that the TabPages collection property of the current tab control has changed.
            RaiseComponentChanging(TypeDescriptor.GetProperties(parentControl)["TabPages"]);
            _designerHost.DestroyComponent(parentControl.SelectedTab);
            RaiseComponentChanged(TypeDescriptor.GetProperties(parentControl)["TabPages"], oldTabs, parentControl.TabPages);
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            GCMTabControl parentControl = e.Component as GCMTabControl;

            if (parentControl != null && e.Member.Name == "TabPages")
            {
                foreach (DesignerVerb verb in Verbs)
                {
                    if (verb.Text == "Remove Tab")
                    {
                        switch (parentControl.TabPages.Count)
                        {
                            case 0:
                                verb.Enabled = false;
                                break;
                            default:
                                verb.Enabled = true;
                                break;
                        }

                        break;
                    }
                }
            }
        }

        #endregion
    }
}
