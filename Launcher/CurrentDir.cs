using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    class CurrentDir
    {
        public ObservableCollection<string> Directories { get; set; }
        public string Path { get; set; }

        public CurrentDir()
        {
            Directories = new ObservableCollection<string>();

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady == false) continue;

                Directories.Add(drive.Name);
            }
        }

        public void Back()
        {
            if (Path != null)
            {
                DirectoryInfo parentDir = Directory.GetParent(Path);
                Path = parentDir.FullName;
                ChangeDir(Path);
            }
        }

        public void ChangeDir(string path)
        {
            Path = path;

            var directories = Directory.GetDirectories(Path);

            Directories.Clear();

            foreach (var dir in directories)
            {
                Directories.Add(dir.ToString());
            }
        }
    }
}
