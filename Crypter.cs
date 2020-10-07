using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class Crypter
    {
        string targetPath;
        string sourcePath;
        Logger targetLogger;
        Logger sourceLogger;

        public Crypter(Logger targetLogger, Logger sourceLogger, string targetPath = @"\TargetDirectory", string sourcePath = @"\SourceDirectory")
        {
            this.targetLogger = targetLogger;
            this.sourceLogger = sourceLogger;
            this.targetPath = targetPath;
            this.sourcePath = sourcePath;
        }
        public string Encrypt(string path)
        {
            return "";
        }

        public string Decrypt(string path)
        {
            return "";
        }
    }
}
