
using System;
using System.Collections.Generic;
using System.Drawing;
using PCRBattleRecorder;
using OpenCvSharp;
using PCRBattleRecorder.Config;
using System.Threading;

namespace PCRBattleRecorder.Script
{
    public abstract class ScriptBase
    {

        protected const string ARENA_REFRESH_KEY = "Arena_Refresh";
        protected const string BATTLE_FAILED_GO_BACK_KEY = "Battle_Failed_Go_Back";
        protected const string BATTLE_SPEED_RATE_KEY = "Battle_Speed_Rate";

        protected const string BATTLE_AUTO_OFF_MKEY = "battle_auto_off.png";
        protected const string BATTLE_AUTO_ON_MKEY = "battle_auto_on.png";
        protected const string BATTLE_CHALLENGE_MKEY = "battle_challenge.png";
        protected const string BATTLE_FAILED_MKEY = "battle_failed.png";
        protected const string BATTLE_GOTO_MAIN_STAGELINE_MKEY = "battle_goto_main_stageline.png";
        protected const string BATTLE_NEXT_STEP_MKEY = "battle_next_step.png";
        protected const string BATTLE_SPEED_RATE_1_MKEY = "battle_speed_rate_1.png";
        protected const string BATTLE_SPEED_RATE_2_MKEY = "battle_speed_rate_2.png";
        protected const string BATTLE_SPEED_RATE_4_MKEY = "battle_speed_rate_4.png";
        protected const string BATTLE_START_MKEY = "battle_start.png";
        protected const string BTN_CLOSE_MKEY = "btn_close.png";
        protected const string BTN_CONFIRM_OK_MKEY = "btn_confirm_ok.png";
        protected const string STAGELINE_NEXT_TAG_MKEY = "stageline_next_tag.png";
        protected const string TUTORIAL_ARROW_MKEY = "tutorial_arrow.png";

        private AdbTools adbTools = AdbTools.GetInstance();
        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private LogTools logTools = LogTools.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();
        private OpenCvTools opencvTools = OpenCvTools.GetInstance();
        private PCRTools pcrTools = PCRTools.GetInstance();

        /// <summary>
        /// 单位：毫秒
        /// </summary>
        public virtual int Interval { get; set; } = 2000;

        public virtual bool CanKeepOnWhenException { get; } = false;

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract void OnStart(Mat viewportMat, RECT viewportRect);

        public abstract void Tick(Mat viewportMat, RECT viewportRect);

        public Vec4f GetMatchSourceRectRate(string imgName)
        {
            return GetMatchSourceRectRate(configMgr.PCRRegion.ToString(), imgName);
        }

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

        public OpenCvMatchImageResult GetMatchTemplateResult(Mat viewportMat, RECT viewportRect, string type, string imgName, Vec4f matchSourceRectRate)
        {
            var matchSourceMat = viewportMat.GetChildMatByRectRate(matchSourceRectRate);
            var templateMat = pcrTools.GetResizedTemplateMat(type, imgName);
            var threshold = GetMatchTemplateThreshold(type, imgName);
            var matchResult = opencvTools.MatchImage(matchSourceMat, templateMat, threshold);
            return matchResult;
        }

        public OpenCvMatchImageResult GetMatchTemplateResult(Mat viewportMat, RECT viewportRect, string type, string imgName)
        {
            var matchSourceRectRate = GetMatchSourceRectRate(type, imgName);
            return GetMatchTemplateResult(viewportMat, viewportRect, type, imgName, matchSourceRectRate);
        }

        protected OpenCvMatchImageResult lastMatchResult;

        public bool CanMatchTemplate(Mat viewportMat, RECT viewportRect, string type, string imgName)
        {
            var matchResult = GetMatchTemplateResult(viewportMat, viewportRect, type, imgName);
            lastMatchResult = matchResult;
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

        public bool CanMatchTemplate(Mat viewportMat, RECT viewportRect, string imgName)
        {
            return CanMatchTemplateOfRegion(viewportMat, viewportRect, configMgr.PCRRegion, imgName);
        }

        public bool TryClickTemplateRect(Mat viewportMat, RECT viewportRect, string imgName)
        {
            return TryClickTemplateRect(viewportMat, viewportRect, configMgr.PCRRegion.ToString(), imgName);
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

        public bool TryClickTemplateRect(Mat viewportMat, RECT viewportRect, string imgName, Vec4f matchSourceRectRate)
        {
            return TryClickTemplateRect(viewportMat, viewportRect, configMgr.PCRRegion.ToString(), imgName, matchSourceRectRate);
        }

        public bool TryClickTemplateRect(Mat viewportMat, RECT viewportRect, string type, string imgName, Vec4f matchSourceRectRate)
        {
            var matchResult = GetMatchTemplateResult(viewportMat, viewportRect, type, imgName, matchSourceRectRate);
            if (!matchResult.Success)
                return false;
            var rectToViewport = matchResult.GetMatchedAbsoluteRect(viewportRect, matchSourceRectRate);
            var centerPos = rectToViewport.GetCenterPos();
            var emulatorPoint = mumuTools.GetEmulatorPoint(viewportRect, centerPos);
            mumuTools.DoClick(emulatorPoint);
            return true;
        }

        public bool TryClickTutorialArrow(Mat viewportMat, RECT viewportRect)
        {
            if (CanMatchTemplate(viewportMat, viewportRect, TUTORIAL_ARROW_MKEY))
            {
                var matchRes = lastMatchResult;
                var rectRate = GetMatchSourceRectRate(TUTORIAL_ARROW_MKEY);
                var absoluteRect = matchRes.GetMatchedAbsoluteRect(viewportRect, rectRate);
                var pos = absoluteRect.GetCenterPos();
                pos.Y = pos.Y + (int)(viewportRect.Height * 0.1700f);
                var emulatorPoint = mumuTools.GetEmulatorPoint(viewportRect, pos);
                mumuTools.DoClick(emulatorPoint);
                logTools.Debug("TryClickTutorialArrow", "TryClickTutorialArrow");
                return true;
            }
            return false;
        }

        public PCRBattleSpeedRate GetBattleSpeedRate(Mat viewportMat, RECT viewportRect)
        {
            if (CanMatchTemplate(viewportMat, viewportRect, BATTLE_SPEED_RATE_1_MKEY))
                return PCRBattleSpeedRate.Rate1;
            else if (CanMatchTemplate(viewportMat, viewportRect, BATTLE_SPEED_RATE_2_MKEY))
                return PCRBattleSpeedRate.Rate2;
            else if (CanMatchTemplate(viewportMat, viewportRect, BATTLE_SPEED_RATE_4_MKEY))
                return PCRBattleSpeedRate.Rate4;
            return PCRBattleSpeedRate.Unknown;
        }

        public Func<Mat, RECT, bool> GetSimpleBattleHandler()
        {
            var func = new Func<Mat, RECT, bool>((viewportMat, viewportRect) =>
            {
                if (TryClickTemplateRect(viewportMat, viewportRect, BATTLE_CHALLENGE_MKEY))
                {
                    logTools.Debug("SimpleBattleHandler", "Try Click BATTLE_CHALLENGE");
                    return true;
                }
                else if (TryClickTemplateRect(viewportMat, viewportRect, BATTLE_START_MKEY))
                {
                    logTools.Debug("SimpleBattleHandler", "Try Click BATTLE_START");
                    return true;
                }
                else if (TryClickTemplateRect(viewportMat, viewportRect, BATTLE_NEXT_STEP_MKEY))
                {
                    logTools.Debug("SimpleBattleHandler", "Try Click BATTLE_START");
                    return true;
                }
                else if (CanMatchTemplate(viewportMat, viewportRect, BATTLE_FAILED_MKEY))
                {
                    logTools.Debug("SimpleBattleHandler", "BATTLE_FAILED");
                    OnBattleFailed(viewportMat, viewportRect);
                    return true;
                }
                return false;
            });
            return func;
        }

        public virtual void OnBattleFailed(Mat viewportMat, RECT viewportRect)
        {
            mumuTools.DoClick(BATTLE_FAILED_GO_BACK_KEY);
            Thread.Sleep(2000);
            ClickTab(viewportRect, PCRTab.Character); //挑战失败 前往角色
        }

        public Func<Mat, RECT, bool> GetSimpleBattleSceneHandler(bool autoBattle, PCRBattleSpeedRate speedRate)
        {
            var func = new Func<Mat, RECT, bool>((viewportMat, viewportRect) =>
            {
                var matchAutoOn = CanMatchTemplate(viewportMat, viewportRect, BATTLE_AUTO_ON_MKEY);
                var matchAutoOff = CanMatchTemplate(viewportMat, viewportRect, BATTLE_AUTO_OFF_MKEY);
                var isBattleScene = matchAutoOn || matchAutoOff;

                if (isBattleScene)
                {
                    logTools.Debug("SimpleBattleHandler", "In BattleScene");

                    if (autoBattle && matchAutoOff) TryClickTemplateRect(viewportMat, viewportRect, BATTLE_AUTO_ON_MKEY);
                    if (!autoBattle && matchAutoOn) TryClickTemplateRect(viewportMat, viewportRect, BATTLE_AUTO_OFF_MKEY);

                    var speedRate1 = GetBattleSpeedRate(viewportMat, viewportRect);
                    if (speedRate != speedRate1) mumuTools.DoClick(BATTLE_SPEED_RATE_KEY);

                }

                return isBattleScene;
            });
            return func;
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
