using System;
using System.Collections.Generic;
using System.IO;
using PCRBattleRecorder.Config;

namespace PCRBattleRecorder
{
    class LogTools
    {
        private static LogTools instance;

        public static LogTools GetInstance()
        {
            if (instance == null)
            {
                instance = new LogTools();
            }
            return instance;
        }

        public event Action<string, string> OnInfo;

        public event Action<string, string> OnError;

        private ConfigMgr configMgr = ConfigMgr.GetInstance();

        private LogTools()
        {
        }

        public void Error(string tag, string msg, bool writeIntoFile)
        {
            OnError?.Invoke(tag, msg);
            if (writeIntoFile)
            {
                AppendIntoFile(ConfigMgr.GetInstance().ErrorLogPath, $"[{tag}] {msg}");
            }
        }

        public void Error(string tag, string msg)
        {
            Error(tag, msg, true);
        }

        public void Error(string tag, Exception e)
        {
            var ex = e.InnerException ?? e;

            var noTrack = IsSelfOrChildrenNoTrackTraceException(ex);
            Error(tag, ex.Message, !noTrack);
            if (!noTrack)
            {
                Error(tag, ex.StackTrace);
            }
        }

        public void Info(string tag, string msg)
        {
            OnInfo?.Invoke(tag, msg);
        }

        public void AppendIntoFile(string filePath, string s)
        {
            using (var file = new FileStream(filePath, FileMode.Append))
            {
                using (var writer = new StreamWriter(file))
                {
                    var time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff");
                    writer.WriteLine($"[{time}] {s}");
                }
            }
        }

        public bool IsSelfOrChildrenBreakException(Exception e)
        {
            if (e is BreakException) return true;
            if (e.InnerException is BreakException) return true;
            return false;
        }

        public bool IsSelfOrChildrenNoTrackTraceException(Exception e)
        {
            if (e is NoTrackTraceException) return true;
            if (e.InnerException is NoTrackTraceException) return true;
            return false;
        }
    }
}
