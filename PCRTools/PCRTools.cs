
using System;
using System.Collections.Generic;
using OpenCvSharp;
using CvSize = OpenCvSharp.Size;
using PCRBattleRecorder.Config;
using ConfigBase = PCRBattleRecorder.Config.Config;
using Newtonsoft.Json.Linq;

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

        private const string MSRectRateJsonFileName = "match_source_rect_rate.json";
        private ConfigBase parentMSRectRateData = JsonConfig.FromFileOrEmpty(FileTools.GetInstance().JoinPath(ConfigMgr.GetInstance().PCRDataDir, "Template", MSRectRateJsonFileName));

        private Dictionary<string, ConfigBase> childMSRectRateDataDict = new Dictionary<string, ConfigBase>();

        public ConfigBase GetMatchSourceRectRateData(string key)
        {
            //缓存
            if (childMSRectRateDataDict.ContainsKey(key))
                return childMSRectRateDataDict[key];
            var data = JsonConfig.FromFileOrEmpty(fileTools.JoinPath(configMgr.PCRDataDir, "Template", key, MSRectRateJsonFileName));
            childMSRectRateDataDict[key] = data; //缓存
            return data;
        }

        public Vec4f GetRectRateByJArray(JArray jArr)
        {
            var getFloat = new Func<int, float>((index) =>
            {
                return jArr[index].Value<float>();
            });
            return new Vec4f(getFloat(0), getFloat(1), getFloat(2), getFloat(3));
        }

        public Vec4f GetTemplateMatchSourceRectRate(string type, string imgName)
        {
            var rectRateData = GetMatchSourceRectRateData(type);
            try
            {
                if (rectRateData.HasKey(imgName))
                {
                    var jArr = rectRateData.Get(imgName) as JArray;
                    return GetRectRateByJArray(jArr);
                }
                else
                {
                    var jArr = parentMSRectRateData.Get(imgName) as JArray;
                    return GetRectRateByJArray(jArr);
                }
            }
            catch (Exception e)
            {
                throw new Exception(Trans.T("无法读取 {0}.{0} 样图的采样区域", type, imgName));
            }
        }

        public string GetTemplateImgPath(string relativePath)
        {
            var imgDir = fileTools.JoinPath(configMgr.PCRDataDir, "Template");
            var path = fileTools.JoinPath(imgDir, relativePath);
            return path;
        }

        public string GetTemplateImgPath(string type, string imgName)
        {
            var imgDir = fileTools.JoinPath(configMgr.PCRDataDir, "Template", type);
            var path = fileTools.JoinPath(imgDir, imgName);
            return path;
        }

        public Mat GetTemplateMat(string relativePath)
        {
            var path = GetTemplateImgPath(relativePath);
            var mat = OpenCvExtension.ReadMatFromFile(path);
            return mat;
        }

        public Mat GetTemplateMat(string type, string imgName)
        {
            var path = GetTemplateImgPath(type, imgName);
            var mat = OpenCvExtension.ReadMatFromFile(path);
            return mat;
        }

        public Mat GetResizedTemplateMat(string relativePath)
        {
            var mat = GetTemplateMat(relativePath);
            var resized = ResizedByTemplateViewportSize(mat);
            return resized;
        }

        public Mat GetResizedTemplateMatOfCommon(string imgName)
        {
            return GetResizedTemplateMat("Common", imgName);
        }

        public Mat GetResizedTemplateMatOfDefaultRegion(string imgName)
        {
            var region = configMgr.PCRRegion;
            return GetResizedTemplateMat(region.ToString(), imgName);
        }

        public Mat GetResizedTemplateMat(string type, string imgName)
        {
            var mat = GetTemplateMat(type, imgName);
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
