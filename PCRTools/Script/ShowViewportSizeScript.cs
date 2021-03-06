﻿using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PCRBattleRecorder.Script
{
    public class ShowViewportSizeScript : ScriptBase
    {

        public override string Name
        {
            get { return "ShowViewportSizeScript"; }
        }

        public override string Description
        {
            get { return Trans.T("跟踪Mumu视口大小"); }
        }

        public override int Interval { get; set; } = 500;

        public override bool CanKeepOnWhenException { get; } = true;

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
        }

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {
            //viewportCapture.Save("capture.png", System.Drawing.Imaging.ImageFormat.Png);
            var s = $"Size: {viewportMat.Width},{viewportMat.Height} Rect: {viewportRect.x1},{viewportRect.y1},{viewportRect.x2},{viewportRect.y2}";
            LogTools.GetInstance().Info("ShowViewportSizeScriptTick", s);
        }
    }
}
