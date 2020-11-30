using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;
using System.IO;

namespace FileWatcherService
{
    class ETLOptions : Options
    {
        protected Parser.Parser parser;
        public string ConfigPath { get; set; }
        string targetPath;
        public string TargetPath
        {
            get { return targetPath; }

            set
            {
                targetPath = value;
                CrypterOptions.TargetPath = value;
                ArchiverOptions.SourcePath = value;
                ArchivePath = System.IO.Path.Combine(value, "archive");
            }
        }
        string sourcePath;
        public string SourcePath
        {
            get { return sourcePath; }
            set
            {
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                CrypterOptions.SourcePath = value;
                sourcePath = value;
            }
        }
        string archivePath;
        public string ArchivePath
        {
            get { return archivePath; }
            set
            {
                ArchiverOptions.TargetPath = value;
                archivePath = value;
            }
        }
        string logPath;
        public string LogPath
        {
            get { return logPath; }
            set
            {
                LoggerOptions.Path = value;
                logPath = value;
            }
        }
        //public Logger Logger { get; set; }
        string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = FixDirName(value);
                TargetPath = System.IO.Path.Combine(path, "TargetDirectory");
                SourcePath = System.IO.Path.Combine(path, "SourceDirectory");
                LogPath = System.IO.Path.Combine(path, "Log.txt");
            }
        }
        string FixDirName(string dir)
        {
            if (dir.Contains(":"))
                return dir;
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);
        }
        public CrypterOptions CrypterOptions { get; set; }
        public ArchiverOptions ArchiverOptions { get; set; }
        public LoggerOptions LoggerOptions { get; set; }
        public ETLOptions()
        {
            CrypterOptions = new CrypterOptions();
            ArchiverOptions = new ArchiverOptions();
            LoggerOptions = new LoggerOptions();
            Path = AppDomain.CurrentDomain.BaseDirectory;
          //  Logger = new Logger(LoggerOptions);
            //CrypterOptions.TargetLogger = Logger;
            //ArchiverOptions.TargetLogger = Logger;

        }
         virtual public void Load()
        {
            //Logger.RecordEntry("Были установлены стандартные настройки");
        }
        protected void Copy(ETLOptions obj)
        {
            if(obj.ConfigPath != null)
                ConfigPath = obj.ConfigPath;
            if(obj.Path != null)
                Path = obj.Path;
        }
    }
}
