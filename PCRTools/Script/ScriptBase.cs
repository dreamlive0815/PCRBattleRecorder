
using System;
using System.Collections.Generic;
using System.Drawing;
using PCRBattleRecorder;
using OpenCvSharp;
using PCRBattleRecorder.Config;
using PCRBattleRecorder.PCRModel;
using System.Threading;
using System.Threading.Tasks;

namespace PCRBattleRecorder.Script
{
    public abstract class ScriptBase
    {

        protected const string ACT_ENTRANCE_KEY = "Act_Entrance";
        protected const string ACT_LIKABILITY_ENTRANCE_KEY = "Act_Likability_Entrance";
        protected const string ARENA_REFRESH_KEY = "Arena_Refresh";
        protected const string BATTLE_FAILED_GO_BACK_KEY = "Battle_Failed_Go_Back";
        protected const string BATTLE_SPEED_RATE_KEY = "Battle_Speed_Rate";
        protected const string CHOICE_2_KEY = "Choice_2";
        protected const string DOWNLOAD_WITHOUT_VOICE_KEY = "Download_Without_Voice";
        protected const string GO_BACK_KEY = "Go_Back";

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
        protected const string BATTLE_UNIT_LIST_VIEW_MKEY = "battle_unit_list_view";
        protected const string BTN_CANCEL_MKEY = "btn_cancel.png";
        protected const string BTN_CLOSE_MKEY = "btn_close.png";
        protected const string BTN_CONFIRM_OK_MKEY = "btn_confirm_ok.png";
        protected const string DATA_DOWNLOAD_TITLE_MKEY = "data_download_title.png";
        protected const string LIKABILITY_ITEM_NEW_TAG_MKEY = "likability_item_new_tag.png";
        protected const string LIKABILITY_PREVIEW_TITLE_MKEY = "likability_preview_title.png";
        protected const string LIKABILITY_TITLE_MKEY = "likability_title.png";
        protected const string LIST_ITEM_NEW_TAG_MKEY = "list_item_new_tag.png";
        protected const string MENU_TAG_MKEY = "menu_tag.png";
        protected const string SKIP_LABEL_MKEY = "skip_label.png";
        protected const string SKIP_TAG_MKEY = "skip_tag.png";
        protected const string STAGELINE_NEXT_TAG_MKEY = "stageline_next_tag.png";
        protected const string STORY_GUIDE_TAG_MKEY = "story_guide_tag.png";
        protected const string STORY_ITEM_NEW_TAG_MKEY = "story_item_new_tag.png";
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

        public double GetMatchTemplateThreshold(string imgName)
        {
            return GetMatchTemplateThreshold(configMgr.PCRRegion.ToString(), imgName);
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

        public bool TryClickListItemNewTag(Mat viewportMat, RECT viewportRect)
        {
            return TryClickTemplateRect(viewportMat, viewportRect, LIST_ITEM_NEW_TAG_MKEY);
        }

        public void DragDownList()
        {
            var startPointRate = new Vec2f(0.7700f, 0.7012f);
            var endPointRate = new Vec2f(0.7700f, 0.2332f);
            mumuTools.DoDrag(startPointRate, endPointRate, 1200);
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
                    logTools.Debug("SimpleBattleHandler", "Try Click BATTLE_NEXT_STEP");
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

        public void ClickBack()
        {
            mumuTools.DoClick(GO_BACK_KEY);
        }


        public virtual void OnBattleTeamUnitNotFound(List<PCRUnit> units, bool[] vis)
        {
        }

        protected Vec4f unitAvatarPartialRectRate = new Vec4f(0.00f, 0.25f, 1.00f, 0.75f);

        public void SelectBattleTeam(Mat viewportMat, RECT viewportRect, List<PCRUnit> units)
        {
            for (var i = 1; i <= 5; i++)
            {
                var key = $"Battle_Team_Slot_{i}";
                mumuTools.DoClick(key);
                Thread.Sleep(500);
            }

            viewportRect = mumuTools.GetMumuViewportRect();
            var viewportCapture = Tools.GetInstance().DoCaptureScreen(viewportRect);
            viewportMat = viewportCapture.ToOpenCvMat();

            var rectRate = GetMatchSourceRectRate(BATTLE_UNIT_LIST_VIEW_MKEY);
            var threshold = GetMatchTemplateThreshold(BATTLE_UNIT_LIST_VIEW_MKEY);
            var vis = new bool[5];
            var dragTimesLimit = 5;
            for (var i = 0; i < dragTimesLimit; i++)
            {
                var matchSourceMat = viewportMat.GetChildMatByRectRate(rectRate);
                for (var k = 0; k < units.Count; k++)
                {
                    if (vis[k]) continue;
                    var unit = units[k];
                    var avatar = unit.GetResizedAvatar();
                    var avatarPartial = avatar.GetChildMatByRectRate(unitAvatarPartialRectRate);
                    var matchRes = opencvTools.MatchImage(matchSourceMat, avatarPartial, threshold);
                    if (matchRes.Success)
                    {
                        var rectToViewport = matchRes.GetMatchedAbsoluteRect(viewportRect, rectRate);
                        var centerPos = rectToViewport.GetCenterPos();
                        var emulatorPoint = mumuTools.GetEmulatorPoint(viewportRect, centerPos);
                        mumuTools.DoClick(emulatorPoint);
                        vis[k] = true;
                        Thread.Sleep(500);
                    }
                }

                var allOk = true;
                for (var k = 0; k < units.Count; k++) allOk = allOk && vis[k];
                if (allOk)
                    break;

                var milliSeconds = 2000;
                mumuTools.DoDrag(new Vec2f(0.5f, 0.6344f), new Vec2f(0.5f, 0.2450f), milliSeconds);
                Thread.Sleep(milliSeconds);

                if (i != dragTimesLimit - 1)
                {
                    viewportRect = mumuTools.GetMumuViewportRect();
                    viewportCapture = Tools.GetInstance().DoCaptureScreen(viewportRect);
                    viewportMat = viewportCapture.ToOpenCvMat();
                }
            }

            OnBattleTeamUnitNotFound(units, vis);
        }

        private Dictionary<string, Mat> unitAvatarPartialDict = new Dictionary<string, Mat>();

        protected void ClearUnitAvatarPartialDict()
        {
            foreach (var pair in unitAvatarPartialDict)
            {
                pair.Value.Dispose();
            }
            unitAvatarPartialDict.Clear();
        }

        protected Mat GetUnitAvatarPartial(string unitID, PCRAvatarLevel avatarLevel, int templateWidth)
        {
            var key = $"{unitID}_{avatarLevel}_{templateWidth}";
            if (unitAvatarPartialDict.ContainsKey(key))
                return unitAvatarPartialDict[key];
            var unit = PCRUnit.FromUnitID(unitID, avatarLevel.GetRequiredStars());
            var avatar = unit.GetResizedAvatar(avatarLevel, templateWidth);
            var avatarPartial = avatar.GetChildMatByRectRate(unitAvatarPartialRectRate);
            unitAvatarPartialDict[key] = avatarPartial;
            return avatarPartial;
        }

        private object getUnitAvatarPartialLock = new object();

        public PCRUnit GetBattleTeamInfoByIndex(Mat viewportMat, RECT viewportRect, int index)
        {
            var key = $"Battle_Team_Slot_{index}";
            var rectRate = GetMatchSourceRectRate(key);
            var threshold = GetMatchTemplateThreshold("Battle_Team_Slot");
            var matchSourceMat = viewportMat.GetChildMatByRectRate(rectRate);
            var unitIDs = PCRUnit.GetAllUnitIDs();
            var templateWidth = configMgr.UnitAvatarTemplateSize.Width;
            var getByAvatarLevel = new Func<string, PCRAvatarLevel, PCRUnit>((unitID, avatarLevel) =>
            {
                if (unitID == PCRUnit.UnknownUnitID)
                    return null;
                Mat avatarPartial;
                lock (getUnitAvatarPartialLock)
                {
                    avatarPartial = GetUnitAvatarPartial(unitID, avatarLevel, templateWidth);
                }
                var matchRes = opencvTools.MatchImage(matchSourceMat, avatarPartial, threshold);
                if (matchRes.Success)
                    return PCRUnit.FromUnitID(unitID, avatarLevel.GetRequiredStars());
                else
                    return null;
            });
            var getByAvatarLevelNoEx = new Func<string, PCRAvatarLevel, PCRUnit>((unitID, avatarLevel) =>
            {
                try { return getByAvatarLevel(unitID, avatarLevel); }
                catch { return null; }
            });
            foreach (var unitID in unitIDs)
            {
                PCRUnit unit;
                unit = getByAvatarLevelNoEx(unitID, PCRAvatarLevel.Level3);
                if (unit != null) return unit;
                unit = getByAvatarLevelNoEx(unitID, PCRAvatarLevel.Level1);
                if (unit != null) return unit;
                unit = getByAvatarLevelNoEx(unitID, PCRAvatarLevel.Level6);
                if (unit != null) return unit;
            }
            return null;
        }

        public List<PCRUnit> GetBattleTeamInfo(Mat viewportMat, RECT viewportRect)
        {
            var list = new List<PCRUnit>();
            //for (var i = 1; i <= 5; i++)
            //{
            //    var unit = GetBattleTeamInfoByIndex(viewportMat, viewportRect, i);
            //    list.Add(unit);
            //}
            var tasks = new Task[5];
            for (var i = 0; i < 5; i++)
            {
                list.Add(null);
                tasks[i] = new Task((idxO) =>
                {
                    var idx = (int)idxO;
                    var unit = GetBattleTeamInfoByIndex(viewportMat, viewportRect, idx + 1);
                    list[idx] = unit;
                }, i);
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            //ClearUnitAvatarPartialDict();
            return list;
        }

        private int GetPowerBarPercent(Mat barMat, int threshold)
        {
            var gray = barMat.ToGray();
            var egY = gray.Height / 2;
            var cnt = 0;
            for (var c = 0; c < gray.Cols; c++)
            {
                var clr = gray.GetPixel(egY, c);
                if (clr.R > threshold) cnt++;
            }
            var percent = (int)(100.0 * cnt / gray.Cols);
            //gray.SaveImage(configMgr.GetCacheFullPath("barGray.png"));
            return percent;
        }

        public List<int> GetBattleSceneUnitHPPercents(Mat viewportMat, RECT viewportRect)
        {
            var r = new List<int>();
            for (var i = 1; i <= 5; i++)
            {
                var percent = GetBattleSceneUnitHPPercent(viewportMat, viewportRect, i);
                r.Add(percent);
            }
            return r;
        }

        public int GetBattleSceneUnitHPPercent(Mat viewportMat, RECT viewportRect, int index)
        {
            var rectRate = GetMatchSourceRectRate($"Battle_Unit_HP_{index}");
            var mat = viewportMat.GetChildMatByRectRate(rectRate);
            var percent = GetPowerBarPercent(mat, 100);
            return percent;
        }

        public List<int> GetBattleSceneUnitTPPercents(Mat viewportMat, RECT viewportRect)
        {
            var r = new List<int>();
            for (var i = 1; i <= 5; i++)
            {
                var percent = GetBattleSceneUnitTPPercent(viewportMat, viewportRect, i);
                r.Add(percent);
            }
            return r;
        }

        public int GetBattleSceneUnitTPPercent(Mat viewportMat, RECT viewportRect, int index)
        {
            var rectRate = GetMatchSourceRectRate($"Battle_Unit_TP_{index}");
            var mat = viewportMat.GetChildMatByRectRate(rectRate);
            var percent = GetPowerBarPercent(mat, 100);
            return percent;
        }

        public List<bool> IsBattleSceneUnitsUBReady(Mat viewportMat, RECT viewportRect)
        {
            var r = new List<bool>();
            for (var i = 1; i <= 5; i++)
            {
                var isReady = IsBattleSceneUnitUBReady(viewportMat, viewportRect, i);
                r.Add(isReady);
            }
            return r;
        }

        public bool IsBattleSceneUnitUBReady(Mat viewportMat, RECT viewportRect, int index)
        {
            var rectRate = GetMatchSourceRectRate($"Battle_Unit_UB_OK_{index}");
            var matchSourceMat = viewportMat.GetChildMatByRectRate(rectRate);
            var threshold = GetMatchTemplateThreshold("Battle_Unit_UB_OK");
            var templateMat = pcrTools.GetResizedTemplateMat("battle_unit_ub_ok.png");
            var matchRes = opencvTools.MatchImage(matchSourceMat, templateMat, threshold);
            return matchRes.Success;
        }

        public List<PCRUnitStatus> GetBattleSceneUnitsStatus(Mat viewportMat, RECT viewportRect)
        {
            List<bool> ubStatus = null;
            List<int> hpPercents = null;
            List<int> tpPercents = null;
            var tasks = new Task[3];
            tasks[0] = Task.Run(() =>
            {
                ubStatus = IsBattleSceneUnitsUBReady(viewportMat, viewportRect);
            });
            tasks[1] = Task.Run(() =>
            {
                hpPercents = GetBattleSceneUnitHPPercents(viewportMat, viewportRect);
            });
            tasks[2] = Task.Run(() =>
            {
                tpPercents = GetBattleSceneUnitTPPercents(viewportMat, viewportRect);
            });
            Task.WaitAll(tasks);
            var r = new List<PCRUnitStatus>();
            for (var i = 0; i < 5; i++)
            {
                r.Add(new PCRUnitStatus()
                {
                    index = i + 1,
                    UBReady = ubStatus[i],
                    HPPercent = hpPercents[i],
                    TPPercent = tpPercents[i]
                });
            }
            return r;
        }
    }

    public struct PCRUnitStatus
    {
        public int index;
        public bool UBReady;
        public int HPPercent;
        public int TPPercent;
    }
}
