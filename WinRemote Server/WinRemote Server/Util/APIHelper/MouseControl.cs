using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinRemote_Server.Util
{
    class MouseControl
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private struct POINT
        {
            public long x;
            public long y;
        }

        public static Point GetCursorPos()
        {
            POINT p = new POINT();
            GetCursorPos(out p);
            return new Point((int)p.x, (int)p.y);
        }

        public static void MoveMouse(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void LeftMouseClick(int x, int y)
        {
            LeftMouseClick(x, y, 0);
        }

        public static void LeftMouseClick(int x, int y, int length)
        {
            Thread t = new Thread(() => ThreadedLeftMouseClick(x, y, length));
        }

        private static void ThreadedLeftMouseClick(int x, int y, int length)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            Thread.Sleep(length);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }
    }
}
