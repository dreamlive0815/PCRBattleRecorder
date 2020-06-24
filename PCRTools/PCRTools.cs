
using System;
using System.Collections.Generic;
using OpenCvSharp;
using CvSize = OpenCvSharp.Size;
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
            var imgDir = fileTools.JoinPath(configMgr.PCRDataDir, "Template");
            var path = fileTools.JoinPath(imgDir, relativePath);
            return path;
        }

        public string GetCommonTemplateImgPath(string imgName)
        {
            var imgDir = fileTools.JoinPath(configMgr.PCRDataDir, "Template", "Common");
            var path = fileTools.JoinPath(imgDir, imgName);
            return path;
        }

        public string GetTemplateImgPathOfRegion(PCRRegion region, string imgName)
        {
            var imgDir = fileTools.JoinPath(configMgr.PCRDataDir, "Template");
            var path = fileTools.JoinPath(imgDir, region.ToString(), imgName);
            return path;
        }

        public Mat GetTemplateMat(string relativePath)
        {
            var path = GetTemplateImgPath(relativePath);
            var mat = OpenCvExtension.ReadMatFromFile(path);
            return mat;
        }

        public Mat GetCommonTemplateMat(string name)
        {
            var path = GetCommonTemplateImgPath(name);
            var mat = OpenCvExtension.ReadMatFromFile(path);
            return mat;
        }

        public Mat GetTemplateMatOfRegion(PCRRegion region, string name)
        {
            var path = GetTemplateImgPathOfRegion(region, name);
            var mat = OpenCvExtension.ReadMatFromFile(path);
            return mat;
        }

        public Mat GetResizedTemplateMat(string relativePath)
        {
            var mat = GetTemplateMat(relativePath);
            var resized = ResizedByTemplateViewportSize(mat);
            return resized;
        }

        public Mat GetResizedCommonTemplateMat(string name)
        {
            var mat = GetCommonTemplateMat(name);
            var resized = ResizedByTemplateViewportSize(mat);
            return resized;
        }

        public Mat GetResizedTemplateMatOfRegion(PCRRegion region, string name)
        {
            var mat = GetTemplateMatOfRegion(region, name);
            var resized = ResizedByTemplateViewportSize(mat);
            return resized;
        }

        private bool bGotViewportSize = false;
        private Size viewportSize; //有缓存的话想更新这个数据必须重启

        public Size GetViewportSize()
        {
            if (bGotViewportSize)
                return viewportSize;
            var viewportRect = MumuTools.GetInstance().GetMumuViewportRect();
            viewportSize = new Size(viewportRect.Width, viewportRect.Height);
            bGotViewportSize = true; //缓存
            return viewportSize;
        }

        public Mat ResizedByTemplateViewportSize(Mat mat)
        {
            var templateSize = configMgr.MumuViewportTemplateSize;
            var realSize = GetViewportSize();
            var widScale = 1.0f * realSize.Width / templateSize.Width;
            var heiScale = 1.0f * realSize.Height / templateSize.Height;
            var resized = mat.Resize(new CvSize(mat.Width * widScale, mat.Height * heiScale));
            return resized;
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
