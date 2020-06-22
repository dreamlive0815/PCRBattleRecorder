
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using PCRBattleRecorder.Config;

namespace PCRBattleRecorder
{
    class PCRTools
    {
        private static PCRTools instance;

        public static PCRTools GetInstance()
        {
            if (instance == null)
            {
                instance = new PCRTools();
            }
            return instance;
        }

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private FileTools fileTools = FileTools.GetInstance();
        private OpenCvTools opencvTools = OpenCvTools.GetInstance();

        private PCRTools()
        {
        }

        public string GetTemplateImgPath(string relativePath)
        {
            var imgDir = configMgr.PCRTemplateImgDir;
            var path = fileTools.JoinPath(imgDir, relativePath);
            return path;
        }

        public string GetTemplateImgPathOfRegion(PCRRegion region, string imgName)
        {
            var imgDir = configMgr.PCRTemplateImgDir;
            var path = fileTools.JoinPath(imgDir, region.ToString(), imgName);
            return path;
        }

        public Mat GetTemplateMatOfRegion(PCRRegion region, string name)
        {
            var path = GetTemplateImgPathOfRegion(region, name);
            var mat = OpenCvExtension.ReadMatFromFile(path);
            return mat;
        }
    }
}
