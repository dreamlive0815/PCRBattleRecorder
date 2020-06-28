
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using RawPoint = System.Drawing.Point;
using OpenCvSharp;
using System.Drawing;
using PCRBattleRecorder.Config;
using System.IO;

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

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private LogTools logTools = LogTools.GetInstance();

        private Tools()
        {
        }

        public string DoShell(string exePath, string arguments)
        {
            return DoShell(exePath, arguments, false);
        }

        public string DoShell(string exePath, string arguments, bool ingoreError)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Minimized,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            var proc = Process.Start(startInfo);
            proc.WaitForExit();
            var output = proc.StandardOutput.ReadToEnd();
            var error = proc.StandardError.ReadToEnd().Trim();
            if (!ingoreError && !string.IsNullOrEmpty(error))
            {
                throw new ShellException(error);
            }
            if (configMgr.DebugMode)
            {
                var fileName = new FileInfo(exePath).Name;
                var strLen = 32;
                var args = arguments.Length <= strLen ? arguments : arguments.Substring(0, strLen) + "...";
                logTools.Info("DoShell", $"[{fileName}] {args} {output}");
            }
            return output;
        }

        public Bitmap DoCaptureScreen(RECT rect)
        {
            if (rect.x1 < 0 || rect.y1 < 0) throw new NoTrackTraceException(Trans.T("左上角坐标不合法"));
            if (rect.x2 < 0 || rect.y2 < 0) throw new NoTrackTraceException(Trans.T("右下角坐标不合法"));
            var width = Math.Abs(rect.x1 - rect.x2);
            var height = Math.Abs(rect.y1 - rect.y2);
            width = Math.Max(width, 10); //下限保护
            height = Math.Max(height, 10); //下限保护
            var bitmap = new Bitmap(width, height);
            var g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(rect.x1, rect.y1, 0, 0, new System.Drawing.Size(width, height));
            g.Dispose();
            return bitmap;
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

        /// <summary>
        /// 特别注意这是相对于RECT的坐标而不是屏幕绝对坐标
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
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
        public const int SB_VERT = 0x1;
        public const int SB_THUMBPOSITION = 0x4;
        public const int WM_VSCROLL = 0x115;


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        public static extern bool PostMessageA(IntPtr hWnd, int nBar, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

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
