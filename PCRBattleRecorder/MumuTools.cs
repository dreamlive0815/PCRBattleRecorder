using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRBattleRecorder
{
    class MumuTools
    {

        private static MumuTools instance;

        public static MumuTools GetInstance()
        {
            if (instance == null)
            {
                instance = new MumuTools();
            }
            return instance;
        }

        private MumuTools()
        {
        }


        public Process GetMumuProcess()
        {
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                var procName = process.ProcessName;
                if (procName.Contains("NemuPlayer"))
                {
                    return process;
                }
            }
            throw new BreakException(Trans.T("无法找到Mumu模拟器进程"));
        }


        private void CheckRect(RECT rect)
        {
            if (rect.x1 < 0 || rect.y1 < 0 || rect.x2 < 0 || rect.y2 < 0)
            {
                throw new Exception(Trans.T("窗口尺寸不合法，可能是因为窗口没有前置显示"));
            }
        }

        public RECT GetMumuRect()
        {
            var proc = GetMumuProcess();
            var rect = Win32Api.GetWindowRect(proc.MainWindowHandle);
            CheckRect(rect);
            return rect;
        }

        public RECT GetMumuViewportRect()
        {
            var proc = GetMumuProcess();
            var hWnd = Win32Api.FindWindowEx(proc.MainWindowHandle, IntPtr.Zero, null, null);
            var viewportRect = Win32Api.GetWindowRect(hWnd);
            CheckRect(viewportRect);
            if (viewportRect.Width <= 15 || viewportRect.Height <= 15)
            {
                throw new Exception(Trans.T("Mumu模拟器窗口尺寸不合法"));
            }
            var title = Win32Api.GetWindowTitle(hWnd);
            if (!title.Contains("NemuPlayer"))
            {
                throw new Exception(Trans.T("无法获取Mumu模拟器窗口尺寸"));
            }
            return viewportRect;
        }
    }
}
