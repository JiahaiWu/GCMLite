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
    [Designer(typeof(System.Windows.Forms.Design.ScrollableControlDesigner))]
    public class TabPageEx : TabPage
    {
        #region Instance Members

        internal bool preventClosing = false;
        private bool _isClosable = true;
        private string _text = null;

        #endregion

        #region Constructor

        public TabPageEx()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ContainerControl, true);

            this.Font = new Font("Arial", 10f);
            this.BackColor = Color.White;
        }

        public TabPageEx(string text)
            : this()
        {
            this.Text = text;
        }

        #endregion

        #region Destructor

        ~TabPageEx()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Property

        /// <summary>
        /// Determines whether the tab page is closable or not.
        /// </summary>
        [Description("Determines whether the tab page is closable or not")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool IsClosable
        {
            get { return _isClosable; }
            set
            {
                if (!value.Equals(_isClosable))
                {
                    _isClosable = value;

                    if (this.Parent != null)
                    {
                        this.Parent.Invalidate();
                        this.Parent.Update();
                    }
                }
            }
        }

        public new string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value != null && !value.Equals(_text))
                {
                    base.Text = value;
                    base.Text = base.Text.Trim();
                    base.Text = base.Text.PadRight(base.Text.Length + 2);
                    _text = base.Text.TrimEnd();
                }
            }
        }

        [DefaultValue(true)]
        [Browsable(true)]
        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
            }
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return this.Text;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            if (this.Parent != null)
                this.Parent.Invalidate();
        }

        #endregion
    }
}
