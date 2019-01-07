using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;

namespace Launcher
{
    class DirBrowser
    {
        public List<string> Paths { get; set; }
        public ObservableCollection<string> DirectoryNames { get; set; }
        public string Path { get; set; }
        public List<Executable> Executables { get; set; }

        public DirBrowser()
        {
            Paths = new List<string>();
            DirectoryNames = new ObservableCollection<string>();

            goToRoot();
        }

        public bool Back()
        {
            if (Path != null)
            {
                DirectoryInfo parentDir = Directory.GetParent(Path);
                if (parentDir == null)
                {
                    goToRoot();
                    return false;
                }
                else
                {
                    Path = parentDir.FullName;
                    ChangeDir(Path);
                }
            }
            return true;
        }

        public bool ChangeDir(string path)
        {
            try
            {
                var directories = Directory.GetDirectories(path);

                Path = path;

                clearPaths();

                foreach (var dir in directories)
                {
                    Paths.Add(dir.ToString());
                    var dirName = new DirectoryInfo(dir).Name;
                    DirectoryNames.Add(dirName);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void goToRoot()
        {
            clearPaths();

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady == false) continue;

                Paths.Add(drive.Name);
                DirectoryNames.Add(drive.Name);
            }
        }

        private void clearPaths()
        {
            Paths.Clear();
            DirectoryNames.Clear();
        }

        public void GetExecutables()
        {
            Executables = new List<Executable>();

            List<string> projectPaths = new List<string>();
            recursiveBrowse(Path, ref projectPaths);

            XNamespace xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";

            foreach (var projectPath in projectPaths)
            {
                XDocument projDefinition = XDocument.Load(projectPath);

                if (projDefinition.Element(xmlns + "Project") == null) continue;

                IEnumerable<XNode> assemblyResultsEnumerable = projDefinition
                    .Element(xmlns + "Project")
                    .Elements(xmlns + "PropertyGroup")
                    .Elements(xmlns + "OutputPath").Nodes<XContainer>();

                IList<XNode> assemblyResults = new List<XNode>(assemblyResultsEnumerable);
                foreach (var assemblyResult in assemblyResults)
                {

                    try
                    {
                        var files = Directory.GetFiles(Directory.GetParent(projectPath).ToString() + @"\" + assemblyResult.ToString());
                        foreach (var file in files)
                        {
                            FileInfo fi = new FileInfo(file);
                            if (fi.Extension == ".exe") Executables.Add(new Executable(file));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void recursiveBrowse(string path, ref List<string> projectPaths)
        {
            try
            {
                var directories = Directory.GetDirectories(path);
                var files = Directory.GetFiles(path);

                foreach (var file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Extension == ".csproj") projectPaths.Add(file);
                }

                foreach (var dir in directories)
                {
                    recursiveBrowse(dir, ref projectPaths);
                }
            } catch
            {

            }
        }
    }
}
