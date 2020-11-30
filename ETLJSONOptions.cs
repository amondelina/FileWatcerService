using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatcherService
{
    class ETLJSONOptions : ETLOptions
    {
        public ETLJSONOptions() : base()
        {
            ConfigPath = System.IO.Path.Combine(Path, "appsettings.json");
            parser = new Parser.JSONParser(ConfigPath);
            Load();
        }
        override public void Load()
        {
            Copy(parser.Get<ETLOptions>());
        }
    }
}
