using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class ETLXMLOptions : ETLOptions
    {
        public ETLXMLOptions() : base()
        {
            ConfigPath = System.IO.Path.Combine(Path, "config.xml"); ;
            parser = new Parser.XMLParser(ConfigPath);
            Load();
        }
        override public void Load()
        {
            Copy(parser.Get<ETLOptions>());
        }
    }
}
