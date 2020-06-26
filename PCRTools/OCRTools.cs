using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenCvSharp;
using PCRBattleRecorder.Config;
using System.Text.RegularExpressions;

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


        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private LogTools logTools = LogTools.GetInstance();
        private Tools tools = Tools.GetInstance();

        private OCRTools()
        {
        }

        public string OCR(Mat mat)
        {
            var lans = GetDefaultTesseractLanguages();
            return OCRByTesseractShell(mat, lans);
        }

        public string OCRByTesseractShell(Mat mat, string languages)
        {
            var configMgr = ConfigMgr.GetInstance();
            var randStr = DateTime.Now.ToString("yyMMddHHmmssffff") + mat.GetHashCode().ToString();
            var ocrImgStorePath = configMgr.GetCacheFullPath($"tesseract_{randStr}.png");
            var resName = $"tesseract_result_{randStr}";
            var ocrResPathForTess = configMgr.GetCacheFullPath($"{resName}");
            var ocrResStorePath = configMgr.GetCacheFullPath($"{resName}.txt");
            mat.SaveImage(ocrImgStorePath);
            var tesseratPath = configMgr.TesseractShellPath;
            try
            {
                var args = string.Format("{0} {1} -l {2} --psm 6", ocrImgStorePath, ocrResPathForTess, languages);
                var output = tools.DoShell(tesseratPath, args, true);
                if (!File.Exists(ocrResStorePath))
                    throw new Exception(Trans.T("找不到OCR的输出文件"));
                var result = File.ReadAllText(ocrResStorePath);
                result = FilterTesseractResult(result);
                logTools.Info("OCRResult", result);
                return result;
            }
            catch (Exception e)
            {
                logTools.Error("OCRByTesseractShell", e.Message);
                return "";
            }
            finally
            {
                if (File.Exists(ocrImgStorePath)) File.Delete(ocrImgStorePath);
                if (File.Exists(ocrResStorePath)) File.Delete(ocrResStorePath);
            }
        }

        private Regex filterTail = new Regex("[\\r\\n\\f]+$");
        public string FilterTesseractResult(string result)
        {
            result = result.Replace(" ", "");
            result = filterTail.Replace(result, "");
            return result;
        }

        public string GetTesseractLanguages(PCRRegion pcrRegion)
        {
            switch (pcrRegion)
            {
                case PCRRegion.Mainland: return "chi_sim+eng";
                //case PCRRegion.Taiwan: return "chi_sim+chi_tra+eng";
                case PCRRegion.Taiwan: return "chi_sim+eng";
                case PCRRegion.Japan: return "jpn+eng";
            }
            return "chi_sim+eng";
        }

        public string GetDefaultTesseractLanguages()
        {
            var pcrRegion = configMgr.PCRRegion;
            return GetTesseractLanguages(configMgr.PCRRegion);
        }
    }
}
