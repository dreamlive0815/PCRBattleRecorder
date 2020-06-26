﻿using System;
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
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logTools.OnError += LogTools_OnError;

            configMgr.PCRRegion = PCRRegion.Taiwan;

            //var viewportRect = mumuTools.GetMumuViewportRect();
            //var viewportCapture = Tools.GetInstance().DoCaptureScreen(viewportRect);
            //var viewportMat = viewportCapture.ToOpenCvMat();
            //Cv2.ImShow("viewportMat", viewportMat);

            var script = new ArenaSearchScript();
            scriptMgr.RunScript(script); 

        }

        private void LogTools_OnError(string arg1, string arg2)
        {
            Console.WriteLine($"[Error] [{arg1}] {arg2}");
        }
    }
}