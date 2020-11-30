using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace FileWatcherService
{
    class Archiver
    {
        public ArchiverOptions Options { get; set; }

        public Archiver(ArchiverOptions options)
        {
            Options = options;
        }
        public string Archive(string path)
        {
            var gzName = path + ".gz";
            var name = Path.GetFileName(path);
            if (File.Exists(gzName))
                File.Delete(gzName);
            using (var sourseStream = new FileStream(path, FileMode.Open))
            {
                using (var targetStream = File.Create(gzName) )
                {
                    using (var compressionStream = new GZipStream(targetStream, Options.CompressionLevel))
                    {
                        sourseStream.CopyTo(compressionStream);
                        Options.TargetLogger.RecordEntry($"Файл {name} архивирован");
                    }
                }
            }
            File.Delete(path);
            return gzName;         
        }
        public string Extract(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var dir = Path.GetDirectoryName(path.Replace(Options.SourcePath, Options.TargetPath));
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var fullName = Path.Combine(dir, name);
            if (File.Exists(fullName))
                File.Delete(fullName);

            using (var sourceStream = new FileStream(path, FileMode.Open))
            {
                using (var targetStream = File.Create(fullName))
                {
                    using (var decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }
            }
            return fullName;
        }
    }
}
