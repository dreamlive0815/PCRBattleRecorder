using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PCRBattleRecorder.Config
{
    public class ConfigMgr
    {
        private static ConfigMgr instance;

        public static ConfigMgr GetInstance()
        {
            if (instance == null)
            {
                instance = new ConfigMgr();
            }
            return instance;
        }

        private FileTools fileTools = FileTools.GetInstance();
        private Config config;

        private ConfigMgr()
        {
            var jsonFileFullPath = FileTools.GetInstance().GetFileFullPath("config.json");
            config = JsonConfig.FromFile(jsonFileFullPath);
        }

        public Config GetConfig()
        {
            return config;
        }

        public bool DebugMode { get; set; } = true;

        public string TesseractShellPath
        {
            get
            {
                var path = config.GetString("TesseractShellPath");
                if (!File.Exists(path))
                {
                    path = SetTesseractShellPathByDialog();
                }
                return path;
            }
            set { config.Set("TesseractShellPath", value); }
        }

        public string SetTesseractShellPathByDialog()
        {
            var path = GetFilePathByDialog(Trans.T("请选择Tesseract程序所在的目录"), "tesseract.exe", "tesseract.exe|tesseract.exe");
            TesseractShellPath = path;
            Save();
            return path;
        }

        public string MumuAdbServerPath
        {
            get
            {
                var path = config.GetString("MumuAdbServerPath");
                if (!File.Exists(path))
                {
                    path = SetMumuAdbServerPathByDialog();
                }
                return path;
            }
            set { config.Set("MumuAdbServerPath", value); }
        }

        public string SetMumuAdbServerPathByDialog()
        {
            var path = GetFilePathByDialog(Trans.T("请选择Mumu模拟器AdbServer程序所在的目录"), "adb_server.exe", "adb_server.exe|adb_server.exe");
            MumuAdbServerPath = path;
            Save();
            return path;
        }

        public string PCRDataDir
        {
            get
            {
                var path = config.GetString("PCRDataDir");
                if (!Directory.Exists(path))
                {
                    path = SetPCRDataDirByDialog();
                }
                return path;
            }
            set { config.Set("PCRDataDir", value); }
        }

        public string SetPCRDataDirByDialog()
        {
            var path = GetDirPathByDialog(Trans.T("请选择保存PCR数据资源的目录"));
            PCRDataDir = path;
            Save();
            return path;
        }

        public string GetFilePathByDialog(string title, string fileName, string filter)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Title = title;
            openDialog.FileName = fileName;
            openDialog.Filter = filter;
            if (openDialog.ShowDialog() != DialogResult.OK)
            {
                throw new SetFilePathException(Trans.T("配置文件路径失败"));
            }
            return openDialog.FileName;
        }

        public string GetDirPathByDialog(string title)
        {
            var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = title;
            if (folderDialog.ShowDialog() != DialogResult.OK)
            {
                throw new SetFilePathException(Trans.T("配置目录路径失败"));
            }
            return folderDialog.SelectedPath;
        }

        public void Save()
        {
            config.Save();
        }

        public string ErrorLogPath
        {
            get
            {
                var dirPath = fileTools.GetDirFullPath("log");
                var path = fileTools.JoinPath(dirPath, "error.log");
                return path;
            }
        }

        public string CacheDir
        {
            get
            {
                var dirPath = fileTools.GetDirFullPath("cache");
                return dirPath;
            }
        }

        public string ScriptPluginDir
        {
            get
            {
                var dirPath = fileTools.GetDirFullPath("plugin");
                return dirPath;
            }
        }

        public string GetCacheFullPath(string relativePath)
        {
            var fullPath = fileTools.JoinPath(CacheDir, relativePath);
            return fullPath;
        }

        public PCRRegion ParsePCRRegion(string s)
        {
            s = s.ToLower();
            switch (s)
            {
                case "mainland": return PCRRegion.Mainland;
                case "taiwan": return PCRRegion.Taiwan;
                case "japan": return PCRRegion.Japan;
            }
            return PCRRegion.Mainland;
        }

        public PCRRegion PCRRegion
        {
            get
            {
                var s = config.GetString("PCRRegion");
                return ParsePCRRegion(s);
            }
            set
            {
                config.Set("PCRRegion", value.ToString());
                Save();
            }
        }

        public Size UnitAvatarTemplateSize
        {
            get { return new Size(115, 115); }
        }

        public Size UnitAvatarResourceSize
        {
            get { return new Size(128, 128); }
        }

        public Size MumuViewportTemplateSize
        {
            get { return new Size(1280, 640); }
        }

        public Size MumuViewportEmulatorSize
        {
            get { return new Size(2160, 1080); }
        }

        public bool OutputAutoScroll
        {
            get
            {
                var s = config.GetString("OutputAutoScroll");
                bool r;
                if (bool.TryParse(s, out r))
                    return r;
                else
                    return false;
            }
            set
            {
                config.Set("OutputAutoScroll", value.ToString());
                Save();
            }
        }

        public double DefaultMatchTemplateThreshold { get; set; } = 0.8;
    }
}
