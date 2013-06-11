using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace MultiSaver
{
    public class User32
    {
        [DllImport("user32.dll")]
        public static extern void SetWindowPos(uint Hwnd, uint Level, int X, int Y, int W, int H, uint Flags);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    }
}
