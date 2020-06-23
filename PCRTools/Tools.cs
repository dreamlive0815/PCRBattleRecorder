
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using RawPoint = System.Drawing.Point;
using OpenCvSharp;

namespace PCRBattleRecorder
{
    public class Tools
    {

        private static Tools instance;

        public static Tools GetInstance()
        {
            if (instance == null)
            {
                instance = new Tools();
            }
            return instance;
        }

        private Tools()
        {
        }

        public string DoShell(string exePath, string arguments)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            var proc = Process.Start(startInfo);
            proc.WaitForExit();
            var output = proc.StandardOutput.ReadToEnd();
            var error = proc.StandardError.ReadToEnd().Trim();
            if (!string.IsNullOrEmpty(error))
            {
                throw new ShellException(error);
            }
            return output;
        }

    }

    public struct RECT
    {
        public int x1;
        public int y1;
        public int x2;
        public int y2;

        public RECT(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public int Width
        {
            get { return Math.Abs(x1 - x2); }
        }

        public int Height
        {
            get { return Math.Abs(y1 - y2); }
        }

        public RawPoint GetChildPointByRate(Vec2f rate)
        {
            var point = new RawPoint();
            point.X = (int)(x1 + Width * rate.Item0);
            point.Y = (int)(y1 + Height * rate.Item1);
            return point;
        }

        public RECT GetChildRectByRate(Vec4f rate)
        {
            RECT rect = new RECT();
            rect.x1 = (int)(Width * rate.Item0);
            rect.y1 = (int)(Height * rate.Item1);
            rect.x2 = (int)(Width * rate.Item2);
            rect.y2 = (int)(Height * rate.Item3);
            return rect;
        }

        public RawPoint GetCenterPos()
        {
            var x = (x1 + x2) / 2;
            var y = (y1 + y2) / 2;
            return new RawPoint(x, y);
        }

    }

    public class Win32Api
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

        public static RECT GetWindowRect(IntPtr hWnd)
        {
            var rect = new RECT();
            GetWindowRect(hWnd, out rect);
            return rect;
        }

        public static string GetWindowTitle(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            StringBuilder windowName = new StringBuilder(length + 1);
            GetWindowText(hWnd, windowName, windowName.Capacity);
            return windowName.ToString();
        }

    }


}
