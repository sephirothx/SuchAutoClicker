using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SuchAutoClicker
{
    public partial class Form1 : Form
    {
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP   = 0x04;

        private Thread _clickThread;
        private bool   _run;

        public int Interval => (int)nud_Interval.Value;

        public Form1()
        {
            InitializeComponent();

            btn_Stop.Enabled = false;
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            StartClickingThread();
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            StopClickingThread();
        }

        public void StartClickingThread()
        {
            if (_run) return;

            btn_Start.Enabled = false;
            btn_Stop.Enabled  = true;

            _clickThread = new Thread(AutoClick) {IsBackground = true};
            _clickThread.Start();
        }

        public void StopClickingThread()
        {
            if (!_run) return;

            btn_Start.Enabled = true;
            btn_Stop.Enabled  = false;

            _run = false;
        }

        private void AutoClick()
        {
            _run = true;

            while (_run)
            {
                int x = Cursor.Position.X;
                int y = Cursor.Position.Y;
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);

                Thread.Sleep(Interval);
            }
        }

        #region Interop

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);

        #endregion
    }
}
