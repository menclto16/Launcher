using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    class Executable
    {
        public string Path { get; set; }
        public Executable(string path)
        {
            Path = path;
        }
    }
}
