using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Threading;

namespace PCRBattleRecorder.Script
{
    public class StagelineBattleScript : ScriptBase
    {

        private LogTools logTools = LogTools.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();
        private PCRTools pcrTools = PCRTools.GetInstance();

        public override string Description
        {
            get { return Trans.T("关卡自动推图"); }
        }

        public override string Name
        {
            get { return "StagelineBattleScript"; }
        }

        public override int Interval { get; set; } = 1000;

        public override bool CanKeepOnWhenException { get; } = true;

        Func<Mat, RECT, bool> defaultHandler;

        Func<Mat, RECT, bool> battleSceneHandler;

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
            defaultHandler = GetSimpleBattleHandler();
            battleSceneHandler = GetSimpleBattleSceneHandler(true, PCRBattleSpeedRate.Rate2);
        }

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {

            if (CanMatchTemplate(viewportMat, viewportRect, TUTORIAL_ARROW_MKEY))
            {
                var matchRes = lastMatchResult;
                var rectRate = GetMatchSourceRectRate(TUTORIAL_ARROW_MKEY);
                var absoluteRect = matchRes.GetMatchedAbsoluteRect(viewportRect, rectRate);
                var pos = absoluteRect.GetCenterPos();
                pos.Y = pos.Y + (int)(viewportRect.Height * 0.1700f);
                var emulatorPoint = mumuTools.GetEmulatorPoint(viewportRect, pos);
                mumuTools.DoClick(emulatorPoint);
            }
            else if (CanMatchTemplate(viewportMat, viewportRect, STAGELINE_NEXT_TAG_MKEY))
            {
                var matchRes = lastMatchResult;
                var rectRate = GetMatchSourceRectRate(STAGELINE_NEXT_TAG_MKEY);
                var absoluteRect = matchRes.GetMatchedAbsoluteRect(viewportRect, rectRate);
                var pos = absoluteRect.GetCenterPos();
                pos.Y = pos.Y + (int)(viewportRect.Height * 0.1200f);
                var emulatorPoint = mumuTools.GetEmulatorPoint(viewportRect, pos);
                mumuTools.DoClick(emulatorPoint);
            }
            else if (TryClickTemplateRect(viewportMat, viewportRect, BTN_CLOSE_MKEY))
            {
                logTools.Debug("StagelineBattle", "Click Close");
            }
            else if (TryClickTemplateRect(viewportMat, viewportRect, BTN_CONFIRM_OK_MKEY))
            {
                logTools.Debug("StagelineBattle", "Click Confirm Ok");
            }
            else if (battleSceneHandler(viewportMat, viewportRect))
            {

            }
            else if (defaultHandler(viewportMat, viewportRect))
            {

            }
            else
            {
                //mumuTools.DoClick(new Vec2f(0.1f, 0.8f));
                ClickTab(viewportRect, PCRTab.Battle);
                Thread.Sleep(2000);
                mumuTools.DoClick(new Vec2f(0.6273f, 0.3891f));
            }
        }
    }
}
