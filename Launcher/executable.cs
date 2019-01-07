using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    class Executable
    {
        public string FilePath { get; set; }
        public string Filename { get; set; }
        private System.Windows.Media.ImageSource icon;
        public Executable(string path)
        {
            FilePath = path;
            FileInfo fi = new FileInfo(FilePath);
            Filename = fi.Name.Replace(fi.Extension, "");
        }

        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                if (icon == null && System.IO.File.Exists(FilePath))
                {
                    using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(FilePath))
                    {
                        icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                  sysicon.Handle,
                                  System.Windows.Int32Rect.Empty,
                                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    }
                }

                return icon;
            }
        }

        public void RunExecutable()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = FilePath;
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(FilePath);
            proc.Start();
        }
    }
}
