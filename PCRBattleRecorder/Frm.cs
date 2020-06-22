using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCRBattleRecorder.Config;
using System.IO;

namespace PCRBattleRecorder
{
    public partial class Frm : Form
    {

        ConfigMgr configMgr = ConfigMgr.GetInstance();
        LogTools logTools = LogTools.GetInstance();

        public Frm()
        {
            InitializeComponent();
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            RegisterLogEvents();
            RefreshRegions();

            var rect = MumuTools.GetInstance().GetMumuRect();

            var b = 0;
            var a = 1 / b;
            
        }

        void RegisterLogEvents()
        {
            logTools.OnInfo += LogTools_OnInfo;
            logTools.OnError += LogTools_OnError;
        }        

        private void LogTools_OnInfo(string tag, string msg)
        {
            txtOutput?.AppendLineThreadSafe($"[{tag}] msg", Color.Black);
        }
        private void LogTools_OnError(string tag, string msg)
        {
            txtOutput?.AppendLineThreadSafe($"[{tag}] msg", Color.Red);
        }

        void RefreshRegions()
        {
            var pcrRegion = configMgr.PCRRegion;
            menuRegionMainland.Checked = pcrRegion == PCRRegion.Mainland;
            menuRegionTaiwan.Checked = pcrRegion == PCRRegion.Taiwan;
            menuRegionJapan.Checked = pcrRegion == PCRRegion.Japan;
            Text = Trans.T("当前区域: {0}", pcrRegion.ToCNString());
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
    }
}
