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
    public class GCMTabControlActionList : DesignerActionList
    {
        #region Instance Members

        private GCMTabControl _linkedControl;
        private IDesignerHost _host;
        private IComponentChangeService _changeService;
        private DesignerActionUIService _designerService;
        private bool _isSupportedAlphaColor = false;

        #endregion

        #region Constructor

        // The constructor associates the control to the smart tag action list.
        public GCMTabControlActionList(GCMTabControl control)
            : base(control)
        {
            _linkedControl = control;
            _host = (IDesignerHost)GetService(typeof(IDesignerHost));
            _changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            _designerService = (DesignerActionUIService)GetService(typeof(DesignerActionUIService));

            this.AutoShow = true;   // When this control will be added to the design area, the smart tag panel will open automatically.
        }

        #endregion

        #region Destructor

        ~GCMTabControlActionList()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Property - DesignerActionPropertyItem

        /* Properties that are targets of DesignerActionPropertyItem entries. */

        public GCMTabControl.TabStyle TabStyles
        {
            get { return _linkedControl.TabStyles; }
            set
            {
                GetPropertyByName("TabStyles").SetValue(_linkedControl, value);
            }
        }

        public GCMTabControl.TabAlignments Alignments
        {
            get { return _linkedControl.Alignments; }
            set
            {
                GetPropertyByName("Alignments").SetValue(_linkedControl, value);
            }
        }

        public Color FirstColor
        {
            get { return _linkedControl.TabGradient.ColorStart; }
            set
            {
                if (!value.Equals(_linkedControl.TabGradient.ColorStart))
                {
                    GCMTabControl.GradientTab oldGradient = _linkedControl.TabGradient;

                    DesignerTransaction transaction = null;
                    try
                    {
                        // Start the transaction.
                        transaction = _host.CreateTransaction("First Color");

                        // Trigger a new ComponentChanging event.
                        _changeService.OnComponentChanging(_linkedControl, GetPropertyByName("TabGradient"));

                        // Set the current value to the control.
                        _linkedControl.TabGradient.ColorStart = value;

                        // Trigger a new ComponentChanged event.
                        _changeService.OnComponentChanged(_linkedControl, GetPropertyByName("TabGradient"), oldGradient, _linkedControl.TabGradient);

                        // Commit the transaction.
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception while changing the value of the FirstColor property, " + ex.Message);
                        if (transaction != null)
                            transaction.Cancel();
                    }
                }
            }
        }

        public Color SecondColor
        {
            get { return _linkedControl.TabGradient.ColorEnd; }
            set
            {
                if (!value.Equals(_linkedControl.TabGradient.ColorEnd))
                {
                    GCMTabControl.GradientTab oldGradient = _linkedControl.TabGradient;

                    DesignerTransaction transaction = null;
                    try
                    {
                        // Start the transaction.
                        transaction = _host.CreateTransaction("Second Color");

                        // Trigger a new ComponentChanging event.
                        _changeService.OnComponentChanging(_linkedControl, GetPropertyByName("TabGradient"));

                        // Set the current value to the control.
                        _linkedControl.TabGradient.ColorEnd = value;

                        // Trigger a new ComponentChanged event.
                        _changeService.OnComponentChanged(_linkedControl, GetPropertyByName("TabGradient"), oldGradient, _linkedControl.TabGradient);

                        // Commit the transaction.
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception while changing the value of the SecondColor property, " + ex.Message);
                        if (transaction != null)
                            transaction.Cancel();
                    }
                }
            }
        }

        public LinearGradientMode GradientMode
        {
            get { return _linkedControl.TabGradient.GradientStyle; }
            set
            {
                if (!value.Equals(_linkedControl.TabGradient.GradientStyle))
                {
                    GCMTabControl.GradientTab oldGradient = _linkedControl.TabGradient;

                    DesignerTransaction transaction = null;
                    try
                    {
                        // Start the transaction.
                        transaction = _host.CreateTransaction("Gradient Mode");

                        // Trigger a new ComponentChanging event.
                        _changeService.OnComponentChanging(_linkedControl, GetPropertyByName("TabGradient"));

                        // Set the current value to the control.
                        _linkedControl.TabGradient.GradientStyle = value;

                        // Trigger a new ComponentChanged event.
                        _changeService.OnComponentChanged(_linkedControl, GetPropertyByName("TabGradient"), oldGradient, _linkedControl.TabGradient);

                        // Commit the transaction.
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception while changing the value of the GradientMode property, " + ex.Message);
                        if (transaction != null)
                            transaction.Cancel();
                    }
                }
            }
        }

        public bool IsSupportedAlphaColor
        {
            get { return _isSupportedAlphaColor; }
            set
            {
                if (!value.Equals(_isSupportedAlphaColor))
                    _isSupportedAlphaColor = value;
            }
        }

        #endregion

        #region Override Methods

        /* Implementation of this abstract method creates smart tag 
               items, associates their targets, and collects into list. */

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            try
            {
                // Creating the action list static headers.
                items.Add(new DesignerActionHeaderItem("Commands"));
                items.Add(new DesignerActionHeaderItem("Appearance"));

                if (!_linkedControl.HeaderVisibility)
                {
                    // Creates other action list headers.
                    items.Add(new DesignerActionHeaderItem("Tab Item Appearance"));

                    items.Add(new DesignerActionPropertyItem("TabStyles", "Tab Styles", "Appearance",
                         "Tab Style"));

                    items.Add(new DesignerActionPropertyItem("Alignments", "Tab Alignments", "Appearance",
                        "Tab Alignment"));

                    items.Add(new DesignerActionPropertyItem("FirstColor", "First Color", "Tab Item Appearance",
                        "First TabItem Color"));

                    items.Add(new DesignerActionPropertyItem("SecondColor", "Second Color", "Tab Item Appearance",
                        "Second TabItem Color"));

                    items.Add(new DesignerActionPropertyItem("GradientMode", "Gradient Mode", "Tab Item Appearance",
                        "Gradient Style"));

                    items.Add(new DesignerActionPropertyItem("IsSupportedAlphaColor", "Support Alpha Color", "Tab Item Appearance",
                        "Supports alpha component for tab item background colors"));

                    items.Add(new DesignerActionMethodItem(this,
                        "RandomizeColors", "Randomize Colors", "Tab Item Appearance",
                        "Randomize TabItem Colors", false));
                }

                items.Add(new DesignerActionMethodItem(this,
                    "HeaderVisibility", "StretchToParent " + (_linkedControl.HeaderVisibility ? "ON" : "OFF"), "Appearance",
                    "Determines whether the active tab is stretched to its parent container or not", false));

                items.Add(new DesignerActionMethodItem(this,
                    "AddTab", "Add Tab", "Commands",
                    "Add a new tab page to the container", false));

                if (_linkedControl.TabCount > 0)
                {
                    DesignerActionMethodItem methodRemove = new DesignerActionMethodItem(this, "RemoveTab", "Remove Tab", "Commands",
                        "Removes the selected tab page from the container", false);

                    items.Add(methodRemove);
                }

                // Add a new static header and its items.
                items.Add(new DesignerActionHeaderItem("Information"));
                items.Add(new DesignerActionTextItem("X: " + _linkedControl.Location.X + ", " + "Y: " + _linkedControl.Location.Y, "Information"));
                items.Add(new DesignerActionTextItem("Width: " + _linkedControl.Size.Width + ", " + "Height: " + _linkedControl.Size.Height, "Information"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while generating the action list panel for this KRBTabControl, " + ex.Message);
            }

            return items;
        }

        #endregion

        #region Helper Methods

        /* This helper method to retrieve control properties.
           GetProperties ensures undo and menu updates to work properly. */

        private PropertyDescriptor GetPropertyByName(String propName)
        {
            if (propName != null)
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(_linkedControl)[propName];

                if (prop != null)
                    return prop;
                else
                    throw new ArgumentException("Property name not found.", propName);
            }
            else
                throw new ArgumentNullException("Property name must not blank.");
        }

        #endregion

        #region General Methods - DesignerActionMethodItem

        /* Methods that are targets of DesignerActionMethodItem entries. */

        public void RandomizeColors()
        {
            DesignerTransaction transaction = null;
            try
            {
                // Start the transaction.
                transaction = _host.CreateTransaction("Randomize Colors");

                // Trigger a new ComponentChanging event.
                _changeService.OnComponentChanging(_linkedControl, GetPropertyByName("TabGradient"));

                Color a, b;
                Random rand = new Random();

                if (!IsSupportedAlphaColor)
                {
                    a = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                    b = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                }
                else
                {
                    a = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255), rand.Next(255));
                    b = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255), rand.Next(255));
                }

                _linkedControl.TabGradient.ColorEnd = a;
                _linkedControl.TabGradient.ColorStart = b;

                // Trigger a new ComponentChanged event.
                _changeService.OnComponentChanged(_linkedControl, GetPropertyByName("TabGradient"), null, null);

                // Commit the transaction.
                transaction.Commit();

                // Update Smart Tag Panel.
                _designerService.Refresh(Component);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while doing randomize action, " + ex.Message);
                if (transaction != null)
                    transaction.Cancel();
            }
        }

        public void HeaderVisibility()
        {
            try
            {
                GetPropertyByName("HeaderVisibility").SetValue(_linkedControl, !_linkedControl.HeaderVisibility);
                _designerService.Refresh(Component);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while changing the value of the HeaderVisibility property, " + ex.Message);
            }
        }

        public void AddTab()
        {
            TabControl.TabPageCollection oldTabs = _linkedControl.TabPages;

            DesignerTransaction transaction = null;
            try
            {
                // Start the transaction.
                transaction = _host.CreateTransaction("Add Tab");

                // Trigger a new ComponentChanging event.
                _changeService.OnComponentChanging(_linkedControl, GetPropertyByName("TabPages"));

                // Create a new designer component and add it to the TabPages collection.
                TabPageEx newTab = (TabPageEx)_host.CreateComponent(typeof(TabPageEx));
                newTab.Text = newTab.Name;

                // Add it to the TabPages collection.
                _linkedControl.TabPages.Add(newTab);

                // After than, select the new adding component.
                _linkedControl.SelectedTab = newTab;

                // Trigger a new ComponentChanged event.
                _changeService.OnComponentChanged(_linkedControl, GetPropertyByName("TabPages"), oldTabs, _linkedControl.TabPages);

                // Commit the transaction.
                transaction.Commit();

                // Update Smart Tag Panel.
                _designerService.Refresh(Component);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while adding a new TabPage to the tab container, " + ex.Message);
                if (transaction != null)
                    transaction.Cancel();
            }
        }

        public void RemoveTab()
        {
            if (_linkedControl.SelectedIndex < 0)
                return;

            TabControl.TabPageCollection oldTabs = _linkedControl.TabPages;

            DesignerTransaction transaction = null;
            try
            {
                // Start the transaction.
                transaction = _host.CreateTransaction("Remove Tab");

                // Trigger a new ComponentChanging event.
                _changeService.OnComponentChanging(_linkedControl, GetPropertyByName("TabPages"));

                // Remove the currently selected TabPage from the collection and designer host.
                _host.DestroyComponent(_linkedControl.SelectedTab);

                // Trigger a new ComponentChanged event.
                _changeService.OnComponentChanged(_linkedControl, GetPropertyByName("TabPages"), oldTabs, _linkedControl.TabPages);

                // Commit the transaction.
                transaction.Commit();

                // Update Smart Tag Panel.
                _designerService.Refresh(Component);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while removing the currently selected TabPage from the tab container, " + ex.Message);
                if (transaction != null)
                    transaction.Cancel();
            }
        }

        #endregion
    }
}
