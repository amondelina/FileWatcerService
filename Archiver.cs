using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class Archiver
    {
        string targetPath;
        string sourcePath;
        Logger targetLogger;
        Logger sourceLogger;

        public Archiver(Logger targetLogger, Logger sourceLogger, string targetPath = @"\TargetDirectory", string sourcePath = @"\SourceDirectory")
        {
            this.targetLogger = targetLogger;
            this.sourceLogger = sourceLogger;
            this.targetPath = targetPath;
            this.sourcePath = sourcePath;
        }
        public string Archive(string path)
        {
            return "";
        }
        public string Extract(string path)
        {
            return "";
        }
    }
}
