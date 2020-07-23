using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using PCRBattleRecorder.PCRModel;

namespace PCRBattleRecorder.Script
{
    public class BattleRecordScript : ScriptBase
    {

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

        private void CalUseTime()
        {

            var now = DateTime.Now;
            if (firstCalUseTime)
            {
                preTime = now;
                firstCalUseTime = false;
            }

            var span = now - preTime;
            preTime = now;

            logTools.Debug(Name, $"Tick Takes: {span.Milliseconds} MS");
        }

        private bool hasEnteredBattleScene = false;

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {
            IsPaused = CanMatchTemplate(viewportMat, viewportRect, BATTLE_PAUSE_TAG_MKEY);

            if (paused)
            {
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
                return;
            }


            var unitStatus = GetBattleSceneUnitsStatus(viewportMat, viewportRect);

            CalUseTime();
    
        }

        private void FinishBattleRecord()
        {

        }
    }
}
