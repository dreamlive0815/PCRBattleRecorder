using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Threading;

namespace PCRBattleRecorder.Script
{
    public class UndergroundBattleScript : ScriptBase
    {

        protected const string BTN_UNDERGROUND_CONFIRM_OK_MKEY = "btn_underground_confirm_ok.png";

        private LogTools logTools = LogTools.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();
        private PCRTools pcrTools = PCRTools.GetInstance();

        public override string Description
        {
            get { return Trans.T("地下城自动过关"); }
        }

        public override string Name
        {
            get { return "UndergroundBattleScript"; }
        }

        public override int Interval { get; set; } = 1000;

        public override bool CanKeepOnWhenException { get; } = true;

        Func<Mat, RECT, bool> defaultHandler;

        Func<Mat, RECT, bool> battleSceneHandler;

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
            defaultHandler = GetSimpleBattleHandler();
            battleSceneHandler = GetSimpleBattleSceneHandler(true, PCRBattleSpeedRate.Rate4);
        }

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {

            if (TryClickTutorialArrow(viewportMat, viewportRect))
            {
            }
            else if (TryClickTemplateRect(viewportMat, viewportRect, BTN_UNDERGROUND_CONFIRM_OK_MKEY))
            {
                logTools.Debug("UndergroundBattle", "Try Click BTN_UNDERGROUND_CONFIRM_OK");
            }
            else if (battleSceneHandler(viewportMat, viewportRect))
            {

            }
            else if (defaultHandler(viewportMat, viewportRect))
            {

            }
            else
            {
                
            }
        }


        //public bool TryClickNormalBox(Mat viewportMat, RECT viewportRect)
        //{
        //    var threshold = 0.8;
        //    var rectRate = fullPartRectRate;
        //    var matchRes = MatchImage(viewportMat, viewportRect, rectRate, "underground_normal_box.png", threshold);
        //    if (!matchRes.Success) return false;
        //    var absoluteRect = GetMatchedAbsoluteRect(viewportRect, rectRate, matchRes.MatchedRect);
        //    var left2right = absoluteRect.GetCenterPos().X < 1.0f * viewportRect.Width / 2;
        //    var emulatorPoint = MumuState.GetEmulatorPoint(viewportRect, absoluteRect.GetCenterPos());
        //    MumuState.DoClick(emulatorPoint);
        //    Thread.Sleep(500);
        //    for (int i = 1; i < 3; i++)
        //    {
        //        if (left2right)
        //        {
        //            var newX1Rate = 1.0f * absoluteRect.x2 / viewportRect.Width;
        //            rectRate.Item0 = newX1Rate;
        //        }
        //        else
        //        {
        //            var newX2Rate = 1.0f * absoluteRect.x1 / viewportRect.Width;
        //            rectRate.Item2 = newX2Rate;
        //        }
        //        matchRes = MatchImage(viewportMat, viewportRect, rectRate, "underground_normal_box.png", threshold);
        //        if (!matchRes.Success) break;
        //        absoluteRect = GetMatchedAbsoluteRect(viewportRect, rectRate, matchRes.MatchedRect);
        //        emulatorPoint = MumuState.GetEmulatorPoint(viewportRect, absoluteRect.GetCenterPos());
        //        MumuState.DoClick(emulatorPoint);
        //        Thread.Sleep(500);
        //    }
        //    return true;
        //}

        //public bool TryClickSpecailBox(Mat viewportMat, RECT viewportRect)
        //{
        //    var threshold = 0.8;
        //    var rectRate = fullPartRectRate;
        //    var matchRes = MatchImage(viewportMat, viewportRect, rectRate, "underground_special_box.png", threshold);
        //    if (!matchRes.Success) return false;
        //    var absoluteRect = GetMatchedAbsoluteRect(viewportRect, rectRate, matchRes.MatchedRect);
        //    var left2right = absoluteRect.GetCenterPos().X < 1.0f * viewportRect.Width / 2;
        //    var emulatorPoint = MumuState.GetEmulatorPoint(viewportRect, absoluteRect.GetCenterPos());
        //    MumuState.DoClick(emulatorPoint);
        //    Thread.Sleep(500);
        //    for (int i = 1; i < 3; i++)
        //    {
        //        if (left2right)
        //        {
        //            var newX1Rate = 1.0f * absoluteRect.x2 / viewportRect.Width;
        //            rectRate.Item0 = newX1Rate;
        //        }
        //        else
        //        {
        //            var newX2Rate = 1.0f * absoluteRect.x1 / viewportRect.Width;
        //            rectRate.Item2 = newX2Rate;
        //        }
        //        matchRes = MatchImage(viewportMat, viewportRect, rectRate, "underground_special_box.png", threshold);
        //        if (!matchRes.Success) break;
        //        absoluteRect = GetMatchedAbsoluteRect(viewportRect, rectRate, matchRes.MatchedRect);
        //        emulatorPoint = MumuState.GetEmulatorPoint(viewportRect, absoluteRect.GetCenterPos());
        //        MumuState.DoClick(emulatorPoint);
        //        Thread.Sleep(500);
        //    }
        //    return true;
        //}
    }
}
