using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PCRBattleRecorder;
using PCRBattleRecorder.Config;

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

        public event Action<ScriptBase> OnScriptEnded;

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private FileTools fileTools = FileTools.GetInstance();
        private LogTools logTools = LogTools.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();
        private Tools tools = Tools.GetInstance();

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

        public CreateScriptTaskResult CreateScriptTask(ScriptBase script)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var task = new Task(() =>
            {
                var viewportRect = mumuTools.GetMumuViewportRect();
                var viewportCapture = tools.DoCaptureScreen(viewportRect);
                logTools.Info("ScriptStart", $"Script: {script.Name} OnStart");
                script.OnStart(viewportCapture.ToOpenCvMat(), viewportRect);
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        logTools.Info("ScriptLoop", Trans.T("脚本: {0} 被终止", script.Name));
                        break;
                    }
                    try
                    {
                        Thread.Sleep(script.Interval);
                        viewportRect = mumuTools.GetMumuViewportRect();
                        viewportCapture = tools.DoCaptureScreen(viewportRect);
                        logTools.Info("ScriptLoop", $"Script: {script.Name} Tick");
                        var viewportMat = viewportCapture.ToOpenCvMat();
                        script.Tick(viewportMat, viewportRect);
                    }
                    catch (Exception e)
                    {
                        var needBreak = logTools.IsSelfOrChildrenBreakException(e);
                        if (!script.CanKeepOnWhenException || needBreak)
                        {
                            logTools.Error("ScriptLoop", Trans.T("脚本: {0} 因发生错误或主动结束而被终止", script.Name), false);
                            throw e;
                        }
                        else
                        {
                            logTools.Error("ScriptLoop", Trans.T("脚本: {0} 发生错误", script.Name), false);
                            logTools.Error("ScriptLoop", e);
                        }
                    }
                }
            }, tokenSource.Token);
            task.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                    logTools.Error("ScriptTask", t.Exception);
                else if (t.IsCanceled)
                    logTools.Error("ScriptTask", Trans.T("脚本: {0} 被终止", script.Name));
                OnScriptEnded?.Invoke(script);
            });
            return new CreateScriptTaskResult()
            {
                Task = task,
                Script = script,
                TaskTokenSource = tokenSource,
                TaskToken = token,
            };
        }

        public Task RunScript(ScriptBase script)
        {
            if (HasRunningScript())
            {
                throw new BreakException(Trans.T("当前有脚本: {0} 正在执行", script.Name));
            }
            curScript = script;
            var createRes = CreateScriptTask(script);
            curTask = createRes.Task;
            tokenSource = createRes.TaskTokenSource;
            token = createRes.TaskToken;
            curTask.Start();
            return curTask;
        }

        public void StopCurScript()
        {
            if (!HasRunningScript()) return;
            logTools.Info("StopCurScript", Trans.T("尝试终止当前脚本"));
            tokenSource?.Cancel();
        }

        public ScriptBase CreateScriptFromType(Type scriptType)
        {
            try
            {
                var obj = Activator.CreateInstance(scriptType);
                var script = obj as ScriptBase;
                return script;
            }
            catch (Exception e)
            {
                logTools.Error("CreateScriptFromType", Trans.T("无法从 {0} 类型创建脚本实例: {1}", scriptType.Name, e.Message));
                return null;
            }
        }

        private bool IngoreScriptType(Type scriptType)
        {
            var typeName = scriptType.Name;
            if (typeName == "EmptyScript")
                return true;
            return false;
        }

        public List<Type> GetScriptTypesFromAssembly(Assembly ass)
        {
            var list = new List<Type>();
            var types = ass.GetExportedTypes();
            foreach (var type in types)
            {
                if (IngoreScriptType(type))
                    continue;
                if (type.IsSubclassOf(typeof(ScriptBase)))
                {
                    list.Add(type);
                }
            }
            return list;
        }

        public List<ScriptBase> GetScriptsFromAssembly(Assembly ass)
        {
            var list = new List<ScriptBase>();
            var scriptTypes = GetScriptTypesFromAssembly(ass);
            foreach (var scriptType in scriptTypes)
            {
                var instance = CreateScriptFromType(scriptType);
                if (instance != null)
                {
                    list.Add(instance);
                }
            }
            return list;
        }

        public List<ScriptBase> GetScriptsFromCurrentAssembly()
        {
            var ass = Assembly.GetAssembly(typeof(ScriptBase));
            return GetScriptsFromAssembly(ass);
        }

        public List<ScriptBase> GetScriptsFromPlugin(string pluginDllName)
        {
            var dllFullPath = fileTools.JoinPath(configMgr.ScriptPluginDir, pluginDllName);
            var ass = Assembly.LoadFile(dllFullPath);
            return GetScriptsFromAssembly(ass);
        }

        public List<ScriptBase> GetScriptsFromPluginDir()
        {
            var list = new List<ScriptBase>();
            var filePaths = Directory.GetFiles(configMgr.ScriptPluginDir, "*.dll");
            foreach (var filePath in filePaths)
            {
                var ass = Assembly.LoadFile(filePath);
                var scripts = GetScriptsFromAssembly(ass);
                list.AddRange(scripts);
            }
            return list;
        }

        public List<ScriptBase> GetAllScripts()
        {
            var scripts = GetScriptsFromCurrentAssembly();
            var pluginScripts = GetScriptsFromPluginDir();
            scripts.AddRange(pluginScripts);
            return scripts;
        }

    }

    public struct CreateScriptTaskResult
    {
        public Task Task;
        public ScriptBase Script;
        public CancellationTokenSource TaskTokenSource;
        public CancellationToken TaskToken;
    }
}
