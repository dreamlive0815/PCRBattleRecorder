using System;
using System.Collections.Generic;
using System.Diagnostics;
using PCRBattleRecorder.Config;
using RawPoint = System.Drawing.Point;
using EmulatorPoint = System.Drawing.Point;
using OpenCvSharp;
using System.Drawing;

namespace PCRBattleRecorder
{

    public class MumuTools
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

        private ConfigMgr configMgr = ConfigMgr.GetInstance();

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
                throw new NoTrackTraceException(Trans.T("窗口尺寸不合法，可能是因为窗口没有前置显示"));
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

        public Bitmap DoCaptureViewport()
        {
            var viewportRect = GetMumuViewportRect();
            var capture = Tools.GetInstance().DoCaptureScreen(viewportRect);
            return capture;
        }

        public EmulatorPoint GetEmulatorPoint(Vec2f pointRate)
        {
            var emulatorSize = configMgr.MumuViewportEmulatorSize;
            var emulatorWid = (int)(1.0f * emulatorSize.Width * pointRate.Item0);
            var emulatorHei = (int)(1.0f * emulatorSize.Height * pointRate.Item1);
            return new EmulatorPoint(emulatorWid, emulatorHei);
        }

        public EmulatorPoint GetEmulatorPoint(RECT viewportRect, RawPoint point)
        {
            var pointRate = new Vec2f(1.0f * point.X / viewportRect.Width, 1.0f * point.Y / viewportRect.Height);
            return GetEmulatorPoint(pointRate);
        }
    }
}
