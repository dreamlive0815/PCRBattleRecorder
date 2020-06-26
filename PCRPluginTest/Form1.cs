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

namespace PCRPluginTest
{
    public partial class Form1 : Form
    {
        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private ScriptMgr scriptMgr = ScriptMgr.GetInstance();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            configMgr.PCRRegion = PCRRegion.Taiwan;
            scriptMgr.RunScript(new ArenaSearchScript());
        }
    }
}
