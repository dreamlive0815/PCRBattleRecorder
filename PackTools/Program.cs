using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackTools
{
    class Program
    {
        //static string mode = "Debug";
        static string mode = "Release";

        static string mainProject = "PCRBattleRecorder";

        static List<string> removeUselessFilesProjects = new List<string>()
        {
            mainProject
        };

        static List<string> removeMainProjectDirNames = new List<string>()
        {
            "cache",
            "data",
            "log",
        };

        static List<string> pluginProjects = new List<string>()
        {
            "PCRArenaSearchScript"
        };

        static void Main(string[] args)
        {
            foreach (var projectName in removeUselessFilesProjects)
            {
                RemoveUselessFiles(projectName, mode);
            }

            foreach (var removeMainProjectDirName in removeMainProjectDirNames)
            {
                RemoveMainProjectDir(removeMainProjectDirName, mode);
            }

            foreach (var pluginProjectName in pluginProjects)
            {
                MovePluginDllToMainProject(pluginProjectName, mode);
            }


            MoveMainProjectFilesToPackTemp(mode);
        }

        static string GetDirFullPath(string dirPath, bool create)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            if (create && !dirInfo.Exists)
            {
                dirInfo.Create();
            }
            var fullPath = dirInfo.FullName;
            return fullPath;
        }

        static string JoinPath(params string[] names)
        {
            return string.Join("/", names);
        }

        static string GetSlnDir()
        {
            var dir = Environment.CurrentDirectory;
            dir += "/../../..";
            var slnPath = new DirectoryInfo(dir).FullName;
            return slnPath;
        }

        static string GetProjectOutputDir(string projectName, string mode)
        {
            var slnPath = GetSlnDir();
            var outputDir = JoinPath(slnPath, projectName, "bin", mode);
            return outputDir;
        }

        static bool IsUselessFile(string filePath)
        {
            if (filePath.EndsWith(".xml")) return true;
            if (filePath.EndsWith(".pdb")) return true;
            if (filePath.EndsWith("exe.config")) return true;
            if (filePath.Contains("vshost.exe")) return true;
            return false;
        }

        static void RemoveUselessFiles(string projectName, string mode)
        {

            var outputDir = GetProjectOutputDir(projectName, mode);

            var filePaths = Directory.GetFiles(outputDir);
            foreach (var filePath in filePaths)
            {
                if (IsUselessFile(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        static void RemoveMainProjectDir(string dirName, string mode)
        {
            var outputDir = GetProjectOutputDir(mainProject, mode);
            var dirPath = JoinPath(outputDir, dirName);
            if (!Directory.Exists(dirPath))
                return;

            Directory.Delete(dirPath, true);
        }

        static void MovePluginDllToMainProject(string pluginProjectName, string mode)
        {

            var outputDir = GetProjectOutputDir(mainProject, mode);
            var pluginDir = GetDirFullPath(JoinPath(outputDir, "plugin"), true);

            var pluginOutputDir = GetProjectOutputDir(pluginProjectName, mode);

            var src = JoinPath(pluginOutputDir, $"{pluginProjectName}.dll");
            var dst = JoinPath(pluginDir, $"{pluginProjectName}.dll");

            if (File.Exists(dst)) File.Delete(dst);

            File.Copy(src, dst);
        }


        static void MoveMainProjectFilesToPackTemp(string mode)
        {
            var slnDir = GetSlnDir();
            var packTempDir = JoinPath(slnDir, "PackTemp");
            var dst = JoinPath(packTempDir, mainProject);
            if (Directory.Exists(dst))
                Directory.Delete(dst, true);

            var outputDir = GetProjectOutputDir(mainProject, mode);
            Directory.Move(outputDir, dst);
        }
    }
}
