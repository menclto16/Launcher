using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    class CurrentDir
    {
        public List<String> Directories = new List<String>();
        public string Path { get; set; }

        public CurrentDir()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady == false) continue;

                Directories.Add(drive.Name);
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
