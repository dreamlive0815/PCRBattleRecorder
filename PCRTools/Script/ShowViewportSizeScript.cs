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

        public override void OnStart(Bitmap viewportCapture, RECT viewportRect)
        {
            
        }

        public override void Tick(Bitmap viewportCapture, RECT viewportRect)
        {
            //viewportCapture.Save("capture.png", System.Drawing.Imaging.ImageFormat.Png);
            var s = $"Size: {viewportCapture.Width},{viewportCapture.Height} Rect: {viewportRect.x1},{viewportRect.y1},{viewportRect.x2},{viewportRect.y2}";
            LogTools.GetInstance().Info("ShowViewportSizeScriptTick", s);
        }
    }
}
