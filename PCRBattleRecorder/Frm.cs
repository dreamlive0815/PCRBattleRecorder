﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCRBattleRecorder.Config;
using PCRBattleRecorder.Script;


namespace PCRBattleRecorder
{
    public partial class Frm : Form
    {
        AdbTools adbTools = AdbTools.GetInstance();
        ConfigMgr configMgr = ConfigMgr.GetInstance();
        FileTools fileTools = FileTools.GetInstance();
        LogTools logTools = LogTools.GetInstance();
        MumuTools mumuTools = MumuTools.GetInstance();
        PCRTools pcrTools = PCRTools.GetInstance();
        ScriptMgr scriptMgr = ScriptMgr.GetInstance();

        public Frm()
        {
            InitializeComponent();
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            Text = $"Debug: {configMgr.DebugMode}";

            RegisterLogEvents();
            InitPathConfigs();
            RefreshRegions();
            RefreshOutputAutoScroll();
            RefreshScripts();

            adbTools.OnEvent += AdbTools_OnEvent;
            adbTools.ConnectToMumuAdbServer();
            //adbTools.StartMonitorEvents();

        }

        void RefreshScripts()
        {
            menuScripts.DropDownItems.Clear();
            var scripts = scriptMgr.GetAllScripts();
            foreach (var script in scripts)
            {
                var menuItem = new ToolStripMenuItem(script.Description);
                menuItem.Click += GetScriptMenuItemClickHandler(script);
                menuScripts.DropDownItems.Add(menuItem);
            }
        }

        EventHandler GetScriptMenuItemClickHandler(ScriptBase script)
        {
            var handler = new EventHandler((sender, e) =>
            {
                var newInstance = scriptMgr.CreateScriptFromType(script.GetType());
                scriptMgr.RunScript(newInstance);
            });
            return handler;
        }

        private void AdbTools_OnEvent(AdbEvent obj)
        {
            logTools.Info("AdbTools_OnEvent", $"{obj.Param0} {obj.Param1} {obj.Param2}");
        }

        void RegisterLogEvents()
        {
            logTools.OnInfo += LogTools_OnInfo;
            logTools.OnError += LogTools_OnError;
        }        

        private void LogTools_OnInfo(string tag, string msg)
        {
            txtOutput?.AppendLineThreadSafe($"[{tag}] {msg}", Color.Black);
        }

        private void LogTools_OnError(string tag, string msg)
        {
            txtOutput?.AppendLineThreadSafe($"[{tag}] {msg}", Color.Red);
        }

        void RefreshRegions()
        {
            var pcrRegion = configMgr.PCRRegion;
            menuRegionMainland.Checked = pcrRegion == PCRRegion.Mainland;
            menuRegionTaiwan.Checked = pcrRegion == PCRRegion.Taiwan;
            menuRegionJapan.Checked = pcrRegion == PCRRegion.Japan;
            Text = Trans.T("当前区域: {0}", pcrRegion.ToCNString());
        }

        void RefreshOutputAutoScroll()
        {
            menuOutputAutoScroll.Checked = configMgr.OutputAutoScroll;
        }

        private void menuRegionMainland_Click(object sender, EventArgs e)
        {
            configMgr.PCRRegion = PCRRegion.Mainland;
            RefreshRegions();
        }

        private void menuRegionTaiwan_Click(object sender, EventArgs e)
        {
            configMgr.PCRRegion = PCRRegion.Taiwan;
            RefreshRegions();
        }

        private void menuRegionJapan_Click(object sender, EventArgs e)
        {
            configMgr.PCRRegion = PCRRegion.Japan;
            RefreshRegions();
        }

        void InitPathConfigs()
        {
            var p1 = configMgr.MumuAdbServerPath;
            var p2 = configMgr.TesseractShellPath;
            var d1 = configMgr.PCRDataDir;
        }

        private void menuSetAdbServerPath_Click(object sender, EventArgs e)
        {
            configMgr.SetMumuAdbServerPathByDialog();
        }

        private void menuSetTesseractPath_Click(object sender, EventArgs e)
        {
            configMgr.SetTesseractShellPathByDialog();
        }

        private void menuSetPCRDataDir_Click(object sender, EventArgs e)
        {
            configMgr.SetPCRDataDirByDialog();
        }

        private void menuStopScript_Click(object sender, EventArgs e)
        {
            scriptMgr.StopCurScript();
        }

        private void menuOutputAutoScroll_Click(object sender, EventArgs e)
        {
            configMgr.OutputAutoScroll = !configMgr.OutputAutoScroll;
            RefreshOutputAutoScroll();
        }

        private void menuClearOutput_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
        }

        private void menuOpenCacheDir_Click(object sender, EventArgs e)
        {
            fileTools.OpenDirInExplorer(configMgr.CacheDir);
        }

        private void menuTapPCRMenu_Click(object sender, EventArgs e)
        {
            var viewportRect = mumuTools.GetMumuViewportRect();
            EmptyScript.GetInstance().ClickTab(viewportRect, PCRTab.Menu);
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            var frm = new FrmAbout();
            frm.ShowDialog();
        }


    }
}
