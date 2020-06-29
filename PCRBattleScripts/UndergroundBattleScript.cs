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

        protected const string UNDERGROUND_BOX_NORMAL_MKEY = "underground_box_normal.png";
        protected const string UNDERGROUND_BOX_SPECIAL_MKEY = "underground_box_special.png";

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

        public override int Interval { get; set; } = 2000;

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
            else if (TryClickBoxes(viewportMat, viewportRect))
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

        public bool TryClickBoxes(Mat viewportMat, RECT viewportRect)
        {
            var b1 = TryClickNormalBox(viewportMat, viewportRect);
            var b2 = TryClickSpecialBox(viewportMat, viewportRect);

            return b1 || b2;
        }

        public bool TryClickNormalBox(Mat viewportMat, RECT viewportRect)
        {
            if (TryClickBox(viewportMat, viewportRect, UNDERGROUND_BOX_NORMAL_MKEY))
            {
                logTools.Debug("UndergroundBattle", "Try Click UNDERGROUND_BOX_NORMAL");
                return true;
            }
            return false;
        }

        public bool TryClickSpecialBox(Mat viewportMat, RECT viewportRect)
        {
            if (TryClickBox(viewportMat, viewportRect, UNDERGROUND_BOX_SPECIAL_MKEY))
            {
                logTools.Debug("UndergroundBattle", "Try Click UNDERGROUND_BOX_SPECIAL");
                return true;
            }
            return false;
        }

        public bool TryClickBox(Mat viewportMat, RECT viewportRect, string imgName)
        {
            var rectRate = new Vec4f(0, 0, 1, 1);

            var clicked = false;
            var left2right = false;
            for (int i = 0; i < 3; i++)
            {
                if (!TryClickTemplateRect(viewportMat, viewportRect, imgName, rectRate))
                    break;
                clicked = true;
                var matchRes = lastMatchResult;
                var absoluteRect = matchRes.GetMatchedAbsoluteRect(viewportRect, rectRate);
                if (i == 0)
                    left2right = absoluteRect.GetCenterPos().X < 1.0f * viewportRect.Width / 2;
                if (left2right) //左半部分
                    rectRate.Item0 = 1.0f * absoluteRect.x2 / viewportRect.Width;
                else
                    rectRate.Item2 = 1.0f * absoluteRect.x1 / viewportRect.Width;

                Thread.Sleep(360);
            }

            return clicked;
        }
    }
}
