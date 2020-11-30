using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatcherService
{
    class LoggerOptions : Options
    {
        string path;
        public string Path 
        { 
            get { return path; }
            set
            {
                path = value;
                var dir = System.IO.Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (!File.Exists(path))
                    File.Create(path);
            }
        }
    }
}
