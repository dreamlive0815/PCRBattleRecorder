using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCRBattleRecorder.Config
{
    class ConfigMgr
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

        public string GetFilePathByDialog(string title, string fileName, string filter)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Title = title;
            openDialog.FileName = fileName;
            openDialog.Filter = filter;
            if (openDialog.ShowDialog() != DialogResult.OK)
            {
                throw new GetFilePathException(Trans.T("配置文件路径失败"));
            }
            return openDialog.FileName;
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

        public Size MumuViewportEmulatorSize
        {
            get { return new Size(2160, 1080); }
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

        

    }

    enum PCRRegion
    {
        Mainland,
        Taiwan,
        Japan,
    }

    static class PCRRegionExtension
    {
        public static string ToCNString(this PCRRegion region)
        {
            switch(region)
            {
                case PCRRegion.Mainland: return "国服";
                case PCRRegion.Taiwan: return "台湾";
                case PCRRegion.Japan: return "日本";
            }
            return "未知区域";
        }
    }


}
