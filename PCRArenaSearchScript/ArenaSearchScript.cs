using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using PCRArenaSearchScript;
using PCRBattleRecorder.Config;

namespace PCRBattleRecorder.Script
{
    public class ArenaSearchScript : ScriptBase
    {
        public override string Description
        {
            get { return Trans.T("竞技场寻敌"); }
        }

        public override string Name
        {
            get { return "ArenaSearchScript"; }
        }

        public override bool CanKeepOnWhenException { get; } = true;

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private OCRTools ocrTools = OCRTools.GetInstance();
        private PCRTools pcrTools = PCRTools.GetInstance();
        private string playerRank;
        private string playerName;
        private ArenaSearchOp nameRankOp;

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
            var pcrRegion = configMgr.PCRRegion;
            if (pcrRegion != PCRRegion.Taiwan)
            {
                throw new BreakException(Trans.T("{0}暂时不支持此功能", pcrRegion.ToCNString()));
            }

            var dialog = new PlayerInfoInputDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                throw new BreakException(Trans.T("未输入目标玩家信息"));
            }
            playerRank = dialog.PlayerRank;
            playerName = dialog.PlayerName;
            nameRankOp = dialog.NameRankOp;
        }

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {

            var list = GetPlayerInfoList(viewportMat, viewportRect);
        }

        private List<ArenaPlayerInfo> GetPlayerInfoList(Mat viewportMat, RECT viewportRect)
        {
            var r = new List<ArenaPlayerInfo>();
            for (int i = 0; i < 3; i++)
                r.Add(new ArenaPlayerInfo());
            var tasks = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                var index = i;
                var task = new Task(() =>
                {
                    var name = OCRPlayerName(viewportMat, viewportRect, index);
                    var rank = "";
                    //var name = MumuState.DoArenaPlayerNameOCR(viewportCaptureClone, viewportRect, index);
                    //var rank = MumuState.DoArenaPlayerRankOCR(viewportCaptureClone, viewportRect, index);
                    r[index] = new ArenaPlayerInfo()
                    {
                        Index = index,
                        Name = name,
                        Rank = rank,
                    };
                });
                task.Start();
                tasks[i] = task;
            }
            Task.WaitAll(tasks);
            return r;
        }

        private void ShowMat(string key, Mat mat)
        {
            if (configMgr.DebugMode)
            {
                var storePath = configMgr.GetCacheFullPath($"{key}.png");
                mat.SaveImage(storePath);
            }
        }

        private string OCRPlayerName(Mat viewportMat, RECT viewportRect, int index)
        {
            var rectRate = pcrTools.GetRectRate(configMgr.PCRRegion.ToString(), $"arena_player_name_{index + 1}");

            var playerNameMat = viewportMat.GetChildMatByRectRate(rectRate);
            var grayMat = playerNameMat.ToGray();
            ShowMat("ArenaSearchNameGray", grayMat);
            var r = ocrTools.OCR(grayMat);
            return r;
        }

        private string OCRPlayerRank(Mat viewportMat, RECT viewportRect, int index)
        {
            return "";
        }
    }

    public enum ArenaSearchOp
    {
        And,
        Or,
    }

    public struct ArenaPlayerInfo
    {
        public int Index;
        public string Name;
        public string Rank;
    }
}
