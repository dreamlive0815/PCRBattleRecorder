
using System;
using System.Collections.Generic;
using OpenCvSharp;
using PCRBattleRecorder.Config;

namespace PCRBattleRecorder
{
    public class PCRTools
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

    public enum PCRRegion
    {
        Mainland,
        Taiwan,
        Japan,
    }

    public static class PCRRegionExtension
    {
        public static string ToCNString(this PCRRegion region)
        {
            switch (region)
            {
                case PCRRegion.Mainland: return "国服";
                case PCRRegion.Taiwan: return "台湾";
                case PCRRegion.Japan: return "日本";
            }
            return "未知区域";
        }
    }
}
