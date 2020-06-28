using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PCRBattleRecorder.Script
{
    public class BattleRecordScript : ScriptBase
    {
        public override string Description
        {
            get { return Trans.T("战斗记录脚本"); }
        }

        public override string Name
        {
            get { return "BattleRecordScript"; }
        }

        Func<Mat, RECT, bool> defaultHandler;

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
            defaultHandler = GetSimpleBattleHandler(true, PCRBattleSpeedRate.Rate1);

            
        }

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {

            defaultHandler(viewportMat, viewportRect);

        }
    }
}
