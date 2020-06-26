using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PCRBattleRecorder.Script
{
    public class ArenaSearchScript : ScriptBase
    {
        public override string Description
        {
            get { return Trans.T("竞技场寻敌"); }
        }

        public override string Name
        {
            get { return "ArenaSearchScript"; }
        }

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
            
        }

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {
            

        }
    }
}
