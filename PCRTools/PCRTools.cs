
using System;
using System.Collections.Generic;
using System.IO;
using OpenCvSharp;
using CvSize = OpenCvSharp.Size;
using Newtonsoft.Json.Linq;
using PCRBattleRecorder.Config;
using PCRBattleRecorder.PCRModel;
using ConfigBase = PCRBattleRecorder.Config.Config;

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


        #region 下面是PCR配置文件加载策略 先从主程序同级data目录加载 再从PCRData下childType目录加载 最后从PCRData下parentType目录加载

        private Dictionary<string, ConfigBase> dataContainerDict = new Dictionary<string, ConfigBase>();

        public string GetDataFilePath(string parentType, string childType, string fileName)
        {
            var parentEmpty = string.IsNullOrEmpty(parentType);
            var childEmpty = string.IsNullOrEmpty(childType);
            if (parentEmpty && childEmpty)
                return fileTools.JoinPath(fileTools.GetDirFullPath("data"), fileName); //同级data目录
            else
            {
                if (childEmpty)
                    return fileTools.JoinPath(configMgr.PCRDataDir, parentType, fileName);
                else
                    return fileTools.JoinPath(configMgr.PCRDataDir, parentType, childType, fileName);
            }
        }

        private string GetDictKey(string parentType, string childType, string fileName)
        {
            var parentEmpty = string.IsNullOrEmpty(parentType);
            var childEmpty = string.IsNullOrEmpty(childType);
            if (parentEmpty && childEmpty)
                return fileName;
            else
            {
                if (childEmpty)
                    return $"{parentType}/{fileName}";
                else
                    return $"{parentType}/{childType}/{fileName}";
            }
        }

        public ConfigBase GetDataContainer(string parentType, string childType, string fileName)
        {
            var dictKey = GetDictKey(parentType, childType, fileName);
            var useCache = !configMgr.DebugMode;
            if (useCache && dataContainerDict.ContainsKey(dictKey))
                return dataContainerDict[dictKey]; //缓存
            var filePath = GetDataFilePath(parentType, childType, fileName);
            var dataContainer = JsonConfig.FromFileOrEmpty(filePath);
            if (useCache) dataContainerDict[dictKey] = dataContainer; //缓存
            return dataContainer;
        }

        private object chooseContainerLock = new object();

        public ConfigBase ChooseDataContainer(string parentType, string childType, string fileName, string key)
        {
            lock (chooseContainerLock)
            {
                var relativeDataContainer = GetDataContainer(null, null, fileName); //同级data目录
                if (relativeDataContainer.HasKey(key))
                    return relativeDataContainer;
                var childDataContainer = GetDataContainer(parentType, childType, fileName);
                if (childDataContainer.HasKey(key))
                    return childDataContainer;
                var parentDataContainer = GetDataContainer(parentType, null, fileName);

                return parentDataContainer;
            }
        }

        public string ChooseFilePath(string parentType, string childType, string fileName)
        {
            var relativeFilePath = GetDataFilePath(null, null, fileName);
            if (File.Exists(relativeFilePath))
                return relativeFilePath;
            var childFilePath = GetDataFilePath(parentType, childType, fileName);
            if (File.Exists(childFilePath))
                return childFilePath;
            var parentFilePath = GetDataFilePath(parentType, null, fileName);
            return parentFilePath;
        }

        #endregion

        public Vec2f GetPointRateByJArray(JArray jArr)
        {
            var getFloat = new Func<int, float>((index) =>
            {
                return jArr[index].Value<float>();
            });
            return new Vec2f(getFloat(0), getFloat(1));
        }

        public Vec4f GetRectRateByJArray(JArray jArr)
        {
            var getFloat = new Func<int, float>((index) =>
            {
                return jArr[index].Value<float>();
            });
            return new Vec4f(getFloat(0), getFloat(1), getFloat(2), getFloat(3));
        }

        private const string MatchSourceRectRateJsonFileName = "match_source_rect_rate.json";

        public Vec4f GetTemplateMatchSourceRectRate(string type, string imgName)
        {
            try
            {
                var dataContainer = ChooseDataContainer("Template", type, MatchSourceRectRateJsonFileName, imgName);
                var jArr = dataContainer.Get(imgName) as JArray;
                return GetRectRateByJArray(jArr);
            }
            catch (Exception e)
            {
                throw new BreakException(Trans.T("无法读取 {0}.{1} 样图的采样区域", type, imgName));
            }
        }

        private const string PointRateJsonFileName = "point_rate.json";

        public Vec2f GetPointRate(string type, string key)
        {
            try
            {
                var dataContainer = ChooseDataContainer("Template", type, PointRateJsonFileName, key);
                var jArr = dataContainer.Get(key) as JArray;
                return GetPointRateByJArray(jArr);
            }
            catch (Exception e)
            {
                throw new BreakException(Trans.T("无法读取 {0}.{1} 采样点的位置", type, key));
            }
        }

        private const string RectRateJsonFileName = "rect_rate.json";

        public Vec4f GetRectRate(string key)
        {
            return GetRectRate(configMgr.PCRRegion.ToString(), key);
        }

        public Vec4f GetRectRate(string type, string key)
        {
            try
            {
                var dataContainer = ChooseDataContainer("Template", type, RectRateJsonFileName, key);
                var jArr = dataContainer.Get(key) as JArray;
                return GetRectRateByJArray(jArr);
            }
            catch (Exception e)
            {
                throw new BreakException(Trans.T("无法读取 {0}.{1} 的采样区域比例", type, key));
            }
        }

        private const string MatchTemplateThresholdJsonFileName = "match_template_threshold.json";

        public double GetMatchTemplateThreshold(string type, string imgName)
        {
            try
            {
                var dataContainer = ChooseDataContainer("Template", type, MatchTemplateThresholdJsonFileName, imgName);
                var numStr = dataContainer.Get(imgName).ToString();
                double threshold;
                if (double.TryParse(numStr, out threshold))
                    return threshold;
                else
                    return configMgr.DefaultMatchTemplateThreshold;
            }
            catch (Exception e)
            {
                return configMgr.DefaultMatchTemplateThreshold;
            }
        }

        /// <summary>
        /// 未使用加载策略
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public string GetTemplateImgPath(string relativePath)
        {
            var imgDir = fileTools.JoinPath(configMgr.PCRDataDir, "Template");
            var path = fileTools.JoinPath(imgDir, relativePath);
            return path;
        }

        public string GetTemplateImgPath(string type, string imgName)
        {
            //var imgDir = fileTools.JoinPath(configMgr.PCRDataDir, "Template", type);
            //var path = fileTools.JoinPath(imgDir, imgName);
            var path = ChooseFilePath("Template", type, imgName);
            return path;
        }

        /// <summary>
        /// 未使用加载策略
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 未使用加载策略
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
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
                return viewportSize; //缓存
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

    public enum PCRTab
    {
        Mainpage,
        Character,
        Story,
        Battle,
        Guildhouse,
        Pickup,
        Menu,
    }

    public enum PCRBattleMode
    {
        Mainline,
        Explore,
        Underground,
        Survey,
        Team,
        Arena,
        PrincessArena,
    }

    public enum PCRBattleSpeedRate
    {
        Unknown,
        Rate1,
        Rate2,
        Rate4,
    }

    public enum PCRStory
    {
        Mainline,
        Character,
        Guild,
        Extra,
    }
}
