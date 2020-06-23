using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PCRBattleRecorder;
using PCRBattleRecorder.Config;
using System.Threading;

namespace PCRBattleRecorder.Script
{
    public class ScriptMgr
    {

        private static ScriptMgr instance;

        public static ScriptMgr GetInstance()
        {
            if (instance == null)
            {
                instance = new ScriptMgr();
            }
            return instance;
        }

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();
        private Tools tools = Tools.GetInstance();
        private LogTools logTools = LogTools.GetInstance();
        private Task curTask;
        private ScriptBase curScript;
        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        private ScriptMgr()
        {
        }

        public bool HasRunningScript()
        {
            return curTask != null && curTask.Status == TaskStatus.Running;
        }

        public Task GetCurScriptTask()
        {
            return curTask;
        }

        public Task RunScript(ScriptBase script)
        {
            if (HasRunningScript())
            {
                throw new BreakException(Trans.T("当前有脚本: {0} 正在执行", script.Name));
            }
            curScript = script;
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            curTask = new Task(() =>
            {
                var viewportRect = mumuTools.GetMumuViewportRect();
                var viewportCapture = tools.DoCaptureScreen(viewportRect);
                logTools.Info("RunScript", $"Script: {script.Name} OnStart");
                script.OnStart(viewportCapture, viewportRect);
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        logTools.Info("RunScriptLoop", Trans.T("脚本: {0} 被终止", script.Name));
                        break;
                    }
                    try
                    {
                        Thread.Sleep(script.Interval);
                        viewportRect = mumuTools.GetMumuViewportRect();
                        viewportCapture = tools.DoCaptureScreen(viewportRect);
                        logTools.Info("RunScriptLoop", $"Script: {script.Name} Tick");
                        script.Tick(viewportCapture, viewportRect);
                    }
                    catch (Exception e)
                    {
                        logTools.Error("RunScriptLoop", Trans.T("脚本: {0} 终止或者发生错误", script.Name), false);
                        var needBreak = logTools.IsSelfOrChildrenBreakException(e);
                        if (!script.CanKeepOnWhenException || needBreak)
                            throw e;
                        else
                            logTools.Error("RunScriptLoop", e);
                    }
                }
            }, tokenSource.Token);
            curTask.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                    logTools.Error("RunScript", t.Exception);
                else if (t.IsCanceled)
                    logTools.Error("RunScript", Trans.T("脚本: {0} 被终止", script.Name));
            });
            curTask.Start();
            return curTask;
        }

        public void StopCurScript()
        {
            if (!HasRunningScript()) return;
            logTools.Info("StopCurScript", Trans.T("尝试终止当前脚本"));
            tokenSource?.Cancel();
        }
    }
}
