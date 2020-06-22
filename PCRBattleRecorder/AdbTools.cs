using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCRBattleRecorder.Config;

using EmulatorPoint = System.Drawing.Point;

namespace PCRBattleRecorder
{
    class AdbTools
    {
        private static AdbTools instance;

        public static AdbTools GetInstance()
        {
            if (instance == null)
            {
                instance = new AdbTools();
            }
            return instance;
        }

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private Tools tools = Tools.GetInstance();
        private LogTools logTools = LogTools.GetInstance();

        private AdbTools()
        {
        }

        public void ConnectToMumuAdbServer()
        {
            var output = RunCommand("connect 127.0.0.1:7555");
            logTools.Info("ConnectToMumuAdbServer", output);
        }

        public string GetTapCommand(EmulatorPoint point)
        {
            var cmd = $"shell input tap {point.X} {point.Y}";
            return cmd;
        }

        public string GetDragCommand(EmulatorPoint start, EmulatorPoint end, int milliSeconds)
        {
            var cmd = $"shell input swipe {start.X} {start.Y} {end.X} {end.Y} {milliSeconds}";
            return cmd;
        }

        public string RunCommand(string cmd)
        {
            var output = tools.DoShell(configMgr.MumuAdbServerPath, cmd);
            return output;
        }

        public void DoTap(EmulatorPoint point)
        {
            var cmd = GetTapCommand(point);
            var output = RunCommand(cmd);
        }

        public void DoDrag(EmulatorPoint start, EmulatorPoint end, int milliSeconds)
        {
            var cmd = GetDragCommand(start, end, milliSeconds);
            var output = RunCommand(cmd);
        }
    }
}
