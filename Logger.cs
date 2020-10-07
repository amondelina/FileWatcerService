using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatcherService
{
    class Logger
    {
        string path;
        public Logger(string path)
        {
            var log = new FileInfo(path);
            this.path = (log.FullName);
            RecordEntry("Файл лога " + Path.GetFileName(path) + " был создан");

        }
        public void RecordEntry(string entry)
        {
            using (var writer = File.CreateText(path))
            {
                writer.WriteLine(entry + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                writer.Flush();
            }
        }
    }
}
