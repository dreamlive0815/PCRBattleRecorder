using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCRBattleRecorder.Config;

using EmulatorPoint = System.Drawing.Point;
using System.Diagnostics;

namespace PCRBattleRecorder
{
    public class AdbTools
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

        public event Action<AdbEvent> OnEvent;

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private Tools tools = Tools.GetInstance();
        private LogTools logTools = LogTools.GetInstance();
        private Process adbShellProcess;
        private Task eventMonitorTask;

        private AdbTools()
        {
        }

        public void ConnectToMumuAdbServer()
        {
            string output;
            output = RunCommand("start-server");
            //logTools.Info("ConnectToMumuAdbServer", output);
            output = RunCommand("connect 127.0.0.1:7555");
            //logTools.Info("ConnectToMumuAdbServer", output);
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

        //暂时先这样 不用什么数据结构了
        private string lastLastCode;
        private string lastLastValue;

        private string lastCode;
        private string lastValue;

        private int x;
        private int y;

        private int HexStr2Int(string hexStr)
        {
            var r = Convert.ToInt32($"0x{hexStr}", 16);
            return r;
        }

        //毫秒
        private const int intervalLimitMS = 200;
        private Dictionary<string, long> accessTapEventTimeDict = new Dictionary<string, long>();

        private void AccessNewTapEvent(string type, int x, int y)
        {
            var key = $"{type}_{x}_{y}";
            var nowTime = DateTime.Now.Ticks;
            accessTapEventTimeDict[key] = nowTime;
        }

        private bool CheckAccessNewTapEventTimeLimit(string type, int x, int y)
        {
            var key = $"{type}_{x}_{y}";
            if (!accessTapEventTimeDict.ContainsKey(key))
                return false;
            var lastAccessTime = accessTapEventTimeDict[key];
            var nowTime = DateTime.Now.Ticks;
            var diff = nowTime - lastAccessTime;
            return diff >= intervalLimitMS * 10 * 1000;
        }

        private void HandleEventLine(string line)
        {
            var split = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var getVal = new Func<int, string>((index) =>
            {
                if (index >= split.Length)
                    return "";
                else
                    return split[index];
            });
            var ahead = getVal(0);
            if (!ahead.StartsWith("/dev/input/event"))
                return;
            var type = getVal(1);
            var code = getVal(2);
            if (code == "SYN_REPORT") //过滤
                return;
            var value = getVal(3);

            if (code == "ABS_MT_POSITION_Y" && lastCode == "ABS_MT_POSITION_X"
                 && lastLastCode == "BTN_TOUCH" && (lastLastValue == "UP" || lastLastValue == "DOWN"))
            {
                x = HexStr2Int(lastValue);
                y = HexStr2Int(value);

                var adbEvent = new AdbEvent()
                {
                    Type = AdbEventType.Tap,
                    Param0 = lastLastValue,
                    Param1 = x,
                    Param2 = y,
                };
                OnEvent?.Invoke(adbEvent);
                AccessNewTapEvent(lastLastValue, x, y);
            }
            else if (code == "ABS_MT_TRACKING_ID" && lastCode == "BTN_TOUCH"
                && CheckAccessNewTapEventTimeLimit(lastValue, x, y))
            {
                var adbEvent = new AdbEvent()
                {
                    Type = AdbEventType.Tap,
                    Param0 = lastValue,
                    Param1 = x,
                    Param2 = y,
                };
                OnEvent?.Invoke(adbEvent);
                AccessNewTapEvent(lastLastValue, x, y);
            }

            lastLastCode = lastCode;
            lastLastValue = lastValue;

            lastCode = code;
            lastValue = value;
        }

        public Task StartMonitorEvents()
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = configMgr.MumuAdbServerPath,
                Arguments = "shell getevent -l",
                WindowStyle = ProcessWindowStyle.Minimized,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            adbShellProcess = Process.Start(startInfo);
            eventMonitorTask = new Task(() =>
            {
                var outStream = adbShellProcess.StandardOutput;
                string line;
                while (true)
                {
                    line = outStream.ReadLine();
                    if (line == null)
                        break;
                    HandleEventLine(line);
                }
            });
            eventMonitorTask.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                    logTools.Error("AdbEventMonitor", t.Exception);
            });
            eventMonitorTask.Start();
            return eventMonitorTask;
        }

        public void StopMonitorEvents()
        {
            if (eventMonitorTask != null && eventMonitorTask.Status == TaskStatus.Running)
            {
                eventMonitorTask.Dispose();
            }
            if (adbShellProcess != null && !adbShellProcess.HasExited)
            {
                adbShellProcess.Close();
            }
        }
    }


    public class AdbEvent
    {
        public AdbEventType Type { get; set; }

        public object Param0 { get; set; }

        public object Param1 { get; set; }

        public object Param2 { get; set; }
    }

    public enum AdbEventType
    {
        Tap,
    }
}
