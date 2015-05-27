using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace IDCM.Forms
{
    public partial class MessageDlg : Form
    {
        #region Constructor&Destructor
        public MessageDlg(string dmsg)
        {
            InitializeComponent();
            this.msg=dmsg;
            timestamp = DateTime.Now.Ticks;

            this.wtimer = new System.Windows.Forms.Timer();
            wtimer.Interval = wtick;
            wtimer.Tick += OnTipTimerTick;
            wtimer.Start();
        }
        #endregion
        #region methods
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_NOACTIVATE = 0x08000000;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE;
                return cp;
            }
        }
        #endregion

        #region Events&Handlings
        private void MessageDlg_VisibleChanged(object sender, EventArgs e)
        {
            try{
                if (this.Visible)
                {
                    int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
                    int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height - 20;
                    this.Location = new Point(x, y);//设置窗体在屏幕右下角显示
                    AnimateWinAPI.AnimateWindow(this.Handle, wtick, AnimateWinAPI.AW_SLIDE | AnimateWinAPI.AW_ACTIVATE | AnimateWinAPI.AW_HOR_NEGATIVE);
                }
                //else
                //{
                //    AnimateWinAPI.AnimateWindow(this.Handle, wtick*4, AnimateWinAPI.AW_SLIDE | AnimateWinAPI.AW_HIDE | AnimateWinAPI.AW_HOR_POSITIVE);
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

        private void MessageDlg_Paint(object sender, PaintEventArgs e)
        {
            Graphics grap = e.Graphics;
            Font font = this.Font;
            SizeF size = TextRenderer.MeasureText(msg, font);
            SolidBrush brush = new SolidBrush(Color.RoyalBlue);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            grap.DrawString(msg, font, brush, this.Size.Width / 2, (this.Size.Height - size.Height) / 2, sf);
        }

        private void OnTipTimerTick(object sender, EventArgs e)
        {
            if (LastTime > (wtick * 4))
            {
                Hide();
                wtimer.Stop();
                Dispose();
            }
        }
        #endregion

        #region Property
        public double LastTime
        {
            get
            {
                long lasttime = DateTime.Now.Ticks - timestamp;
                return new TimeSpan(lasttime).TotalMilliseconds;
            }
        }
        #endregion

        #region Members
        private string msg = null;
        private long timestamp = 0L;
        public static readonly int wtick = 700;
        private System.Windows.Forms.Timer wtimer = null;
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #endregion
    }

    /// <summary>
    /// Win32 implementation to show / hide a window with animation.
    /// </summary>
    class AnimateWinAPI
    {
        #region Constants
        /// <summary>
        /// Animates the window from left to right. This flag can be used with roll or slide animation.
        /// </summary>
        public const int AW_HOR_POSITIVE = 0X1;
        /// <summary>
        /// Animates the window from right to left. This flag can be used with roll or slide animation.
        /// </summary>
        public const int AW_HOR_NEGATIVE = 0X2;
        /// <summary>
        /// Animates the window from top to bottom. This flag can be used with roll or slide animation.
        /// </summary>
        public const int AW_VER_POSITIVE = 0X4;
        /// <summary>
        /// Animates the window from bottom to top. This flag can be used with roll or slide animation.
        /// </summary>
        public const int AW_VER_NEGATIVE = 0X8;
        /// <summary>
        /// Makes the window appear to collapse inward if AW_HIDE is used or expand outward if the AW_HIDE is not used.
        /// </summary>
        public const int AW_CENTER = 0X10;
        /// <summary>
        /// Hides the window. By default, the window is shown.
        /// </summary>
        public const int AW_HIDE = 0X10000;
        /// <summary>
        /// Activates the window.
        /// </summary>
        public const int AW_ACTIVATE = 0X20000;
        /// <summary>
        /// Uses slide animation. By default, roll animation is used.
        /// </summary>
        public const int AW_SLIDE = 0X40000;
        /// <summary>
        /// Uses a fade effect. This flag can be used only if hwnd is a top-level window.
        /// </summary>
        public const int AW_BLEND = 0X80000;
        #endregion

        /// <summary>
        /// Animates a window.
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int AnimateWindow(IntPtr hwand, int dwTime, int dwFlags);
    }
}
