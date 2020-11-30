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
                
                archivePath = FixDirName(value);
                ArchiverOptions.TargetPath = archivePath;
            }
        }
        string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = FixDirName(value);
                TargetPath = System.IO.Path.Combine(path, "TargetDirectory");
                SourcePath = System.IO.Path.Combine(path, "SourceDirectory");
                LoggerOptions.Path = System.IO.Path.Combine(path, "Log.txt");
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
        }
         virtual public void Load()
        {
        }
        protected void Copy(ETLOptions obj)
        {
            Path = obj.Path;
            TargetPath = obj.TargetPath;
            SourcePath = obj.SourcePath;
            ArchivePath = obj.ArchivePath;
            Copy(CrypterOptions, obj.CrypterOptions);
            Copy(ArchiverOptions, obj.ArchiverOptions);
            Copy(LoggerOptions, obj.LoggerOptions);   
        }
        void Copy<T>(T obj1, T obj2)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                var prop2 = prop.GetValue(obj2);
                if (prop2 != null)
                    prop.SetValue(obj1, prop2);
            }
        }
    }
}
