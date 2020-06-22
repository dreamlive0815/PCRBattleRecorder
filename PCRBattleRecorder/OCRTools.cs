using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCRBattleRecorder.Config;

namespace PCRBattleRecorder
{
    class OCRTools
    {
        private static OCRTools instance;

        public static OCRTools GetInstance()
        {
            if (instance == null)
            {
                instance = new OCRTools();
            }
            return instance;
        }

        private string tesseractPath = ConfigMgr.GetInstance().TesseractShellPath;

        private OCRTools()
        {
        }
    }
}
