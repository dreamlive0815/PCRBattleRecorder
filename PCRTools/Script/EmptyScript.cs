using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PCRBattleRecorder.Script
{
    public class EmptyScript : ScriptBase
    {

        private static EmptyScript instance;

        public static EmptyScript GetInstance()
        {
            if (instance == null)
            {
                instance = new EmptyScript();
            }
            return instance;
        }

        private EmptyScript()
        {
        }

        public override string Description
        {
            get { return "空脚本,用于调用方法"; }
        }

        public override string Name
        {
            get { return "EmptyScript"; }
        }

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
        }

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {
        }
    }
}
