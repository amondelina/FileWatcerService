using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class Watcher
    {
        public Logger Logger;
        Archiver archiver;
        Crypter crypter;
        FileSystemWatcher watcher;
        FileSystemWatcher jsonWatcher;
        FileSystemWatcher xmlWatcher;
        public Watcher()
        {

            crypter = new Crypter(OptionsManager.GetOptions<CrypterOptions>() as CrypterOptions);
            archiver = new Archiver(OptionsManager.GetOptions<ArchiverOptions>() as ArchiverOptions);
            watcher = new FileSystemWatcher(OptionsManager.Options.SourcePath, "*.txt");

            watcher.Created += OnChanged;
            watcher.Changed += OnChanged;
            watcher.Renamed += OnChanged;
            watcher.Deleted += OnDeleted;

            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;

            Logger = OptionsManager.Logger;
            archiver.Options.TargetLogger = Logger;
            crypter.Options.TargetLogger = Logger;

            jsonWatcher = new FileSystemWatcher(OptionsManager.Options.Path, "appsettings.json");
            jsonWatcher.Created += OnOptionsFileChanged;
            jsonWatcher.Changed += OnOptionsFileChanged;
            jsonWatcher.Deleted += OnOptionsFileChanged;
            jsonWatcher.EnableRaisingEvents = true;

            xmlWatcher = new FileSystemWatcher(OptionsManager.Options.Path, "config.xml");
            xmlWatcher.Created += OnOptionsFileChanged;
            xmlWatcher.Changed += OnOptionsFileChanged;
            xmlWatcher.Deleted += OnOptionsFileChanged;
            if (OptionsManager.Options != OptionsManager.JSONOptions)
                xmlWatcher.EnableRaisingEvents = true;
            else
                xmlWatcher.EnableRaisingEvents = false;
        }
        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            jsonWatcher.EnableRaisingEvents = true;
            while (watcher.EnableRaisingEvents)
                Thread.Sleep(1000);
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            jsonWatcher.EnableRaisingEvents = false;
            xmlWatcher.EnableRaisingEvents = false;
            xmlWatcher.Dispose();
            jsonWatcher.Dispose();
            watcher.Dispose();
        }
        void OnOptionsFileChanged(object o, FileSystemEventArgs e)
        {
            try
            {
                OptionsManager.Update();
            }
            catch(Exception)
            {
                Logger.RecordEntry("Что-то не так с файлами конфигурации");
            }
            
            if (OptionsManager.Options != OptionsManager.JSONOptions)
                xmlWatcher.EnableRaisingEvents = true;
            else
                xmlWatcher.EnableRaisingEvents = false;
        }
        void OnChanged(object o, FileSystemEventArgs e)
        {
            var type = e.ChangeType;
            var path = e.FullPath;
            if (type == WatcherChangeTypes.Created)
                Logger.RecordEntry($"Файл {path} был создан");
            if (type == WatcherChangeTypes.Changed)
                Logger.RecordEntry($"Файл {path} был изменен");
            if (type == WatcherChangeTypes.Renamed)
                Logger.RecordEntry($"Файл {path} был переименован");
     
            var encreptedPath = crypter.Encrypt(path);
            var zipPath = archiver.Archive(encreptedPath);
            var extractedPath = archiver.Extract(zipPath);
            crypter.Decrypt(extractedPath);
        }
        void OnDeleted(object o, FileSystemEventArgs e)
        {
            Logger.RecordEntry($"Файл {e.FullPath} был удален");
        }




    }
}
