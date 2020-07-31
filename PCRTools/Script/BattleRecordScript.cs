using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using PCRBattleRecorder.PCRModel;
using System.Threading;

namespace PCRBattleRecorder.Script
{
    public class BattleRecordScript : ScriptBase
    {
        private const int TICK_INTERVAL_MS = 200;
        private LogTools logTools = LogTools.GetInstance();

        public override string Description
        {
            get { return Trans.T("战斗记录脚本"); }
        }

        public override string Name
        {
            get { return "BattleRecordScript"; }
        }

        public override int Interval { get; set; } = 1;

        public override bool CanKeepOnWhenException { get; } = true;


        private List<PCRUnit> units;

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
            if (!CanMatchTemplate(viewportMat, viewportRect, BATTLE_SET_TEAM_SCENE_TITLE_MKEY))
            {
                throw new BreakException(Trans.T("请在编组队伍界面启动脚本"));
            }

            logTools.Info(Name, Trans.T("正在读取队伍编组..."));
            units = GetBattleTeamInfo(viewportMat, viewportRect);


            TryClickTemplateRect(viewportMat, viewportRect, BATTLE_START_MKEY);
        }

        private object pauseLock = new object();
        private bool paused = false;
        private bool IsPaused
        {
            get
            {
                lock (pauseLock)
                {
                    return paused;
                }
            }
            set
            {
                lock (pauseLock)
                {
                    paused = value;
                }
            }
        }


        private object battleTimeLock = new object();
        private int battleTime = int.MaxValue;
        private int BattleTime
        {
            get
            {
                lock (battleTimeLock)
                {
                    return battleTime;
                }
            }
            set
            {
                lock (battleTimeLock)
                {
                    battleTime = value;
                }
            }
        }


        private bool firstCalUseTime = true;
        private DateTime preTime;

        private int CalUseTimeAndDelay()
        {
            if (firstCalUseTime)
            {
                preTime = DateTime.Now;
                firstCalUseTime = false;
            }
            var span = DateTime.Now - preTime;
            var spanMS = span.Milliseconds;
            logTools.Debug(Name, $"Tick Takes: {spanMS} MS");
            var sleepMS = Math.Max(TICK_INTERVAL_MS - spanMS, 0);
            Thread.Sleep(sleepMS);
            logTools.Debug(Name, $"Tick Sleep: {sleepMS} MS");
            preTime = DateTime.Now;
            return spanMS;
        }

        private bool hasEnteredBattleScene = false;

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {
            IsPaused = CanMatchTemplate(viewportMat, viewportRect, BATTLE_PAUSE_TAG_MKEY);

            if (paused)
            {
                logTools.Debug(Name, $"PAUSED");
                return;
            }

            if (IsBattleScene(viewportMat, viewportRect))
            {
                hasEnteredBattleScene = true;
            }
            else
            {
                if (hasEnteredBattleScene)
                {
                    //throw new BreakException(Trans.T("录制过程请不要离开战斗界面"));
                    FinishBattleRecord();
                }
                logTools.Debug(Name, $"HAS_NOT_ENTERED");
                return;
            }


            var unitStatus = GetBattleSceneUnitsStatus(viewportMat, viewportRect);
            int spanMS = CalUseTimeAndDelay();
    
        }

        private void FinishBattleRecord()
        {


            throw new BreakException(Trans.T("战斗录制结束"));
        }
    }
}
