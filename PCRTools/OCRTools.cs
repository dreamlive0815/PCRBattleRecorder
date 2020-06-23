using System;
using System.Collections.Generic;
using PCRBattleRecorder.Config;

namespace PCRBattleRecorder
{

    public class OCRTools
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
