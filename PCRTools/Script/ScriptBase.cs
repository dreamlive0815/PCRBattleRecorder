
using System;
using System.Collections.Generic;
using System.Drawing;
using PCRBattleRecorder;
using OpenCvSharp;
using PCRBattleRecorder.Config;

namespace PCRBattleRecorder.Script
{
    public abstract class ScriptBase
    {

        private PCRTools pcrTools = PCRTools.GetInstance();
        private OpenCvTools opencvTools = OpenCvTools.GetInstance();
        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();
        private AdbTools adbTools = AdbTools.GetInstance();

        /// <summary>
        /// 单位：毫秒
        /// </summary>
        public virtual int Interval { get; set; } = 2000;

        public virtual bool CanKeepOnWhenException { get; } = false;

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract void OnStart(Mat viewportMat, RECT viewportRect);

        public abstract void Tick(Mat viewportMat, RECT viewportRect);

        public virtual Vec4f GetMatchSourceRectRate(string type, string imgName)
        {
            return pcrTools.GetTemplateMatchSourceRectRate(type, imgName);
        }

        public virtual double GetMatchTemplateThreshold(string type, string imgName)
        {
            return pcrTools.GetMatchTemplateThreshold(type, imgName);
        }

        public virtual Vec2f GetPointRate(string type, string key)
        {
            return pcrTools.GetPointRate(type, key);
        }

        public OpenCvMatchImageResult GetMatchTemplateResult(Mat viewportMat, RECT viewportRect, string type, string imgName)
        {
            var matchSourceRectRate = GetMatchSourceRectRate(type, imgName);
            var matchSourceMat = viewportMat.GetChildMatByRectRate(matchSourceRectRate);
            var templateMat = pcrTools.GetResizedTemplateMat(type, imgName);
            var threshold = GetMatchTemplateThreshold(type, imgName);
            var matchResult = opencvTools.MatchImage(matchSourceMat, templateMat, threshold);
            return matchResult;
        }

        public bool CanMatchTemplate(Mat viewportMat, RECT viewportRect, string type, string imgName)
        {
            var matchResult = GetMatchTemplateResult(viewportMat, viewportRect, type, imgName);
            return matchResult.Success;
        }

        public bool CanMatchCommonTemplate(Mat viewportMat, RECT viewportRect, string imgName)
        {
            return CanMatchTemplate(viewportMat, viewportRect, "Common", imgName);
        }

        public bool CanMatchTemplateOfRegion(Mat viewportMat, RECT viewportRect, PCRRegion region, string imgName)
        {
            return CanMatchTemplate(viewportMat, viewportRect, region.ToString(), imgName);
        }

        public bool CanMatchTemplateOfDefaultRegion(Mat viewportMat, RECT viewportRect, string imgName)
        {
            return CanMatchTemplateOfRegion(viewportMat, viewportRect, configMgr.PCRRegion, imgName);
        }

        public bool TryClickTemplateRect(Mat viewportMat, RECT viewportRect, string type, string imgName)
        {
            var matchResult = GetMatchTemplateResult(viewportMat, viewportRect, type, imgName);
            if (!matchResult.Success)
                return false;
            var matchSourceRectRate = GetMatchSourceRectRate(type, imgName);
            var rectToViewport = matchResult.GetMatchedAbsoluteRect(viewportRect, matchSourceRectRate);
            var centerPos = rectToViewport.GetCenterPos();
            var emulatorPoint = mumuTools.GetEmulatorPoint(viewportRect, centerPos);
            mumuTools.DoClick(emulatorPoint);
            return true;
        }

        public void ClickTab(RECT viewportRect, PCRTab tab)
        {
            ClickTab(viewportRect, configMgr.PCRRegion.ToString(), tab);
        }

        public void ClickTab(RECT viewportRect, string type, PCRTab tab)
        {
            var key = $"PCRTab_{tab}";
            var pointRate = GetPointRate(type, key);
            var emulatorPoint = mumuTools.GetEmulatorPoint(pointRate);
            mumuTools.DoClick(emulatorPoint);
        }
    }


}
