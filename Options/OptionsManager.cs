using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatcherService
{
    class OptionsManager
    {
         public event EventHandler ConfigFileChanged;
        
         public ETLOptions Options { get; set; }
        
        public  Logger Logger { get; set; }
        public string JSONPath { get; set; }
         public string XMLPath { get; set; }
         public ETLXMLOptions XMLOptions { get; set; }
         public ETLJSONOptions JSONOptions { get; set; }
         public ETLOptions DefaultOptions { get; set; }
         public bool xml;
         public bool XML
        {
            get { return xml; }
            set
            {
                xml = value;
                ConfigFileChanged(null, null);
            }
        }
        
        public OptionsManager()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            JSONPath = System.IO.Path.Combine(path, "appsettings.json");
            XMLPath = System.IO.Path.Combine(path, "config.xml");
            DefaultOptions = new ETLOptions();
            Update();
            Logger = new Logger(Options.LoggerOptions);

        }
         public Options GetOptions<T>()
        {
            if (typeof(T) == typeof(LoggerOptions))
                return Options.LoggerOptions;
            if (typeof(T) == typeof(ArchiverOptions))
                return Options.ArchiverOptions;
            if (typeof(T) == typeof(CrypterOptions))
                return Options.CrypterOptions;

           
            return null;
        }
       

         public void Update()
        {
            
            if (File.Exists(JSONPath))
            {
                if (JSONOptions == null)
                    JSONOptions = new ETLJSONOptions();
                Options = JSONOptions;
            }
            else
                if (File.Exists(XMLPath))
            {
                if (XMLOptions == null)
                    XMLOptions = new ETLXMLOptions();
                Options = XMLOptions;
            }
            else
            {
                Options = DefaultOptions;
            }
            Options.Load();
        }


    }
}
