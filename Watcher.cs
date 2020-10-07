using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class Watcher
    {
        Logger sourceLogger;
        //Logger targetLogger;
        Archiver archiver;
        Crypter crypter;
        public Watcher(string path, Logger sourceLogger, Logger targetLogger)
        {
            this.sourceLogger = sourceLogger;
           // this.targetLogger = targetLogger;
            archiver = new Archiver(targetLogger, sourceLogger);
            crypter = new Crypter(targetLogger, sourceLogger);
            using(var watcher = new FileSystemWatcher(path, "*.txt"))
            {
                watcher.Created += OnChanged;
                watcher.Changed += OnChanged;
                watcher.Renamed += OnChanged;
                watcher.Deleted += OnDeleted;

                watcher.EnableRaisingEvents = true;
                watcher.IncludeSubdirectories = true;
            }
        }
        void OnChanged(object o, FileSystemEventArgs e)
        {
            
        }
        void OnDeleted(object o, FileSystemEventArgs e)
        {

        }




    }
}
