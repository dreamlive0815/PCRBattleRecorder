using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PCRBattleRecorder.Script
{
    public class StagelineBattleScript : ScriptBase
    {
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
            if (battleSceneHandler(viewportMat, viewportRect))
            {

            }
            else if (defaultHandler(viewportMat, viewportRect))
            {

            }
        }
    }
}
