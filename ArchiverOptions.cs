using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace FileWatcherService
{
    class ArchiverOptions : Options
    {
        public string TargetPath { get; set; }
        public string SourcePath { get; set; }
        public Logger TargetLogger { get; set; }
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Optimal;

       
    }
}
