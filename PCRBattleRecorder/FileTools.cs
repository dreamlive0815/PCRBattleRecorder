using System;
using System.Diagnostics;
using System.IO;

namespace PCRBattleRecorder
{
    class FileTools
    {
        private static FileTools instance;

        public static FileTools GetInstance()
        {
            if (instance == null)
            {
                instance = new FileTools();
            }
            return instance;
        }

        private FileTools()
        {   
        }

        public string GetFileFullPath(string filePath)
        {
            return GetFileFullPath(filePath, true);
        }

        public string GetFileFullPath(string filePath, bool create)
        {
            var fileInfo = new FileInfo(filePath);
            if (create && !fileInfo.Exists)
            {
                //File.Create(fileInfo.FullName);
                using (var file = new FileStream(fileInfo.FullName, FileMode.CreateNew)) { }
            }
            var fullPath = fileInfo.FullName;
            return fullPath;
        }

        public string GetDirFullPath(string dirPath)
        {
            return GetDirFullPath(dirPath, true);
        }

        public string GetDirFullPath(string dirPath, bool create)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            if (create && !dirInfo.Exists)
            {
                dirInfo.Create();
            }
            var fullPath = dirInfo.FullName;
            return fullPath;
        }

        public string JoinPath(params string[] names)
        {
            return string.Join("/", names);
        }

        public void OpenFileInExplorer(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var fullPath = fileInfo.FullName;
            Process.Start("Explorer.exe", $"/select,{fullPath}");
        }

        public void OpenDirInExplorer(string dirPath)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            var fullPath = dirInfo.FullName;
            Process.Start("Explorer.exe", $"{fullPath}");
        }
    }
}
