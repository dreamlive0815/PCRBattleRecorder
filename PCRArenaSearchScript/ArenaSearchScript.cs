using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using PCRArenaSearchScript;
using PCRBattleRecorder.Config;
using System.Text.RegularExpressions;

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
        private LogTools logTools = LogTools.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();
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
            var playerIndex = FindPlayerIndex(list);
            if (playerIndex != NOT_FOUND)
            {
                logTools.Info("ArenaSearch", $"PlayerIndex: {playerIndex}");
                mumuTools.DoClick($"Arena_Player_{playerIndex + 1}");
                var playerInfo = list[playerIndex];
                throw new BreakException(Trans.T("已找到目标玩家,名字:{0},排名:{1},脚本终止", playerInfo.Name, playerInfo.Rank));
            }
            else
            {
                logTools.Info("ArenaSearch", $"PlayerIndex Not Found");
                mumuTools.DoClick("Arena_Refresh");
            }
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
                    var rank = OCRPlayerRank(viewportMat, viewportRect, index);
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

        private const int NOT_FOUND = -1;

        private int FindPlayerIndex(List<ArenaPlayerInfo> list)
        {
            var namePattern = playerName;
            var rankPattern = playerRank;
            logTools.Info("ArenaSearch", $"Name Pattern: {playerName}");
            logTools.Info("ArenaSearch", $"Rank Pattern: {playerRank}");
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                logTools.Info("ArenaSearch", Trans.T("玩家名字{0}: {1}", i, item.Name));
                var matchName = !string.IsNullOrWhiteSpace(namePattern) && Regex.IsMatch(item.Name, namePattern);
                var matchRank = !string.IsNullOrWhiteSpace(rankPattern) && Regex.IsMatch(item.Rank, rankPattern);
                var op = nameRankOp;
                if (op == ArenaSearchOp.And)
                {
                    if (matchName && matchRank) return i;
                }
                else if (op == ArenaSearchOp.Or)
                {
                    if (matchName || matchRank) return i;
                }
            }
            return NOT_FOUND;
        }

        private void ShowMat(string key, Mat mat)
        {
            if (configMgr.DebugMode)
            {
                var storePath = configMgr.GetCacheFullPath($"{key}.png");
                mat.SaveImage(storePath);
            }
        }

        public string OCRPlayerName(Mat viewportMat, RECT viewportRect, int index)
        {
            var rectRate = pcrTools.GetRectRate(configMgr.PCRRegion.ToString(), $"arena_player_name_{index + 1}");
            var playerNameMat = viewportMat.GetChildMatByRectRate(rectRate);
            var grayMat = playerNameMat.ToGray();
            ShowMat($"ArenaSearchName{index}", grayMat);
            var r = ocrTools.OCR(grayMat);
            return r;
        }

        public string OCRPlayerRank(Mat viewportMat, RECT viewportRect, int index)
        {
            var rectRate = pcrTools.GetRectRate(configMgr.PCRRegion.ToString(), $"arena_player_rank_{index + 1}");
            var playerRankMat = viewportMat.GetChildMatByRectRate(rectRate);
            var grayMat = playerRankMat.ToGray();
            var reverseMat = grayMat.ToReverse();
            var binMat = reverseMat.ToBinary(90);
            binMat = CleanBinCorner(binMat);
            ShowMat($"ArenaSearchRank{index}", binMat);
            var r = ocrTools.OCR(binMat);
            return r;
        }

        private int[] dr = { -1, 0, 1, 0 };
        private int[] dc = { 0, 1, 0, -1 };

        private bool InBounds(Mat mat, int r, int c)
        {
            if (r < 0 || r >= mat.Rows) return false;
            if (c < 0 || c >= mat.Cols) return false;
            return true;
        }

        private void CleanBinCornerDFS(Mat mat, bool[,] vis, int r, int c)
        {
            mat.SetPixel(r, c, 255, 255, 255);
            vis[r, c] = true;
            for (int k = 0; k < 4; k++)
            {
                var nr = r + dr[k];
                var nc = c + dc[k];
                if (InBounds(mat, nr, nc) && !vis[nr, nc])
                {
                    var pix = mat.GetPixel(nr, nc);
                    if (pix.R == 0)
                    {
                        CleanBinCornerDFS(mat, vis, nr, nc);
                    }
                }
            }
        }

        public Mat CleanBinCorner(Mat mat)
        {
            var res = new Mat();
            mat.CopyTo(res);
            var vis = new bool[res.Rows, res.Cols];
            for (int r = 0; r < res.Rows; r++) CleanBinCornerDFS(res, vis, r, 0);
            for (int r = 0; r < res.Rows; r++) CleanBinCornerDFS(res, vis, r, res.Cols - 1);
            for (int c = 0; c < res.Cols; c++) CleanBinCornerDFS(res, vis, 0, c);
            for (int c = 0; c < res.Cols; c++) CleanBinCornerDFS(res, vis, res.Rows - 1, c);
            return res;
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
