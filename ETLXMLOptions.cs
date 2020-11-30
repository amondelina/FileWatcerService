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
            ConfigPath = System.IO.Path.Combine(Path, "appsettings.xml"); ;
        }
        override public void Load()
        {

        }
    }
}
