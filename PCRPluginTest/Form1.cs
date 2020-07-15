using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCRBattleRecorder;
using PCRBattleRecorder.Config;
using PCRBattleRecorder.Script;
using PCRBattleRecorder.PCRModel;
using PCRBattleRecorder.Csv;
using OpenCvSharp;
using System.Threading;

namespace PCRPluginTest
{
    public partial class Form1 : Form
    {
        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private LogTools logTools = LogTools.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();
        private ScriptMgr scriptMgr = ScriptMgr.GetInstance();
        private OCRTools ocrTools = OCRTools.GetInstance();
        private OpenCvTools openCvTools = OpenCvTools.GetInstance();
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logTools.OnError += LogTools_OnError;
            logTools.OnInfo += LogTools_OnInfo;

            configMgr.PCRRegion = PCRRegion.Japan;

            AdbTools.GetInstance().ConnectToMumuAdbServer();

            //var viewportRect = mumuTools.GetMumuViewportRect();
            //var viewportCapture = Tools.GetInstance().DoCaptureScreen(viewportRect);
            //var viewportMat = viewportCapture.ToOpenCvMat();
            //Cv2.ImShow("viewportMat", viewportMat);

            var unit = PCRUnit.FromUnitId(1011);
            //var r = unit.GetResizedAvatar();
            //var matchRes = openCvTools.MatchImage(viewportMat, r, 0.5);

            ScriptBase script;
            script = new StagelineBattleScript();
            //script = new UndergroundBattleScript();
            //script = new ArenaSearchScript();
            //script = new ActLikabilityScript();
            script = new StoryScript();
            scriptMgr.RunScript(script); 
            //var csv = Csv.FromFile("1.csv");
        }

        private void LogTools_OnInfo(string arg1, string arg2)
        {
            Console.WriteLine($"[Info] [{arg1}] {arg2}");
        }

        private void LogTools_OnError(string arg1, string arg2)
        {
            Console.WriteLine($"[Error] [{arg1}] {arg2}");
        }
    }
}
