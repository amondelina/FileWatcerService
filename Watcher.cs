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
        OptionsManager manager;
        public Watcher()
        {
            manager = new OptionsManager();
            crypter = new Crypter(manager.GetOptions<CrypterOptions>() as CrypterOptions);
            archiver = new Archiver(manager.GetOptions<ArchiverOptions>() as ArchiverOptions);
            watcher = new FileSystemWatcher(manager.Options.SourcePath, "*.*");

            watcher.Created += OnChanged;
            watcher.Changed += OnChanged;
            watcher.Renamed += OnChanged;
            watcher.Deleted += OnDeleted;

            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;

            Logger = manager.Logger;
            archiver.Options.TargetLogger = Logger;
            crypter.Options.TargetLogger = Logger;

            jsonWatcher = new FileSystemWatcher(manager.Options.Path, "appsettings.json");
            jsonWatcher.Created += OnOptionsFileChanged;
            jsonWatcher.Changed += OnOptionsFileChanged;
            jsonWatcher.Deleted += OnOptionsFileChanged;
            jsonWatcher.EnableRaisingEvents = true;

            xmlWatcher = new FileSystemWatcher(manager.Options.Path, "config.xml");
            xmlWatcher.Created += OnOptionsFileChanged;
            xmlWatcher.Changed += OnOptionsFileChanged;
            xmlWatcher.Deleted += OnOptionsFileChanged;
            if (manager.Options != manager.JSONOptions)
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
        async void OnOptionsFileChanged(object o, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    manager.Update();
                }
                catch (Exception)
                {
                   Logger.RecordEntry("Что-то не так с файлами конфигурации");
                }

                if (manager.Options != manager.JSONOptions)
                    xmlWatcher.EnableRaisingEvents = true;
                else
                    xmlWatcher.EnableRaisingEvents = false;

            }, null);
            
        }
        void OnChanged(object o, FileSystemEventArgs e)
        {
            var type = e.ChangeType;
            var path = e.FullPath;
            ThreadPool.QueueUserWorkItem((state) => Processing(path, type), null);
        }

        async Task Processing(string path, WatcherChangeTypes type)
        {
            if (type == WatcherChangeTypes.Created)
                await Logger.RecordEntry($"Файл {path} был создан");
            if (type == WatcherChangeTypes.Changed)
                await Logger.RecordEntry($"Файл {path} был изменен");
            if (type == WatcherChangeTypes.Renamed)
                await Logger.RecordEntry($"Файл {path} был переименован");

            var encreptedPath = crypter.Encrypt(path);
            var zipPath = archiver.Archive(encreptedPath.Result);
            var extractedPath = archiver.Extract(zipPath.Result);
            await crypter.Decrypt(extractedPath.Result);
        }
        async void OnDeleted(object o, FileSystemEventArgs e)
        {
            await Logger.RecordEntry($"Файл {e.FullPath} был удален");
        }




    }
}
