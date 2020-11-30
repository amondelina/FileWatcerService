using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatcherService
{
    class CrypterOptions : Options
    {
        public string TargetPath { get; set; }
        public string SourcePath { get; set; }
        public Logger TargetLogger { get; set; }
        public byte[] Key { get; set; } = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

    }
}
