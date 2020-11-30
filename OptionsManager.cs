using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatcherService
{
    static class OptionsManager
    {
        static public event EventHandler ConfigFileChanged;
        
        static public ETLOptions Options { get; set; }
        
        static public  Logger Logger { get; set; }
        static public string JSONPath { get; set; }
        static public string XMLPath { get; set; }
        static public ETLXMLOptions XMLOptions { get; set; }
        static public ETLJSONOptions JSONOptions { get; set; }
        static public ETLOptions DefaultOptions { get; set; }
        static public bool xml;
        static public bool XML
        {
            get { return xml; }
            set
            {
                xml = value;
                ConfigFileChanged(null, null);
            }
        }
        
        static OptionsManager()
        {
            System.Diagnostics.Debugger.Launch();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            JSONPath = System.IO.Path.Combine(path, "appsettings.json");
            XMLPath = System.IO.Path.Combine(path, "config.xml");
            DefaultOptions = new ETLOptions();
            Update();
            Logger = new Logger(Options.LoggerOptions);

        }
        static public Options GetOptions<T>()
        {
            if (typeof(T) == typeof(LoggerOptions))
                return Options.LoggerOptions;
            if (typeof(T) == typeof(ArchiverOptions))
                return Options.ArchiverOptions;
            if (typeof(T) == typeof(CrypterOptions))
                return Options.CrypterOptions;

           
            return null;
        }
       

        static public void Update()
        {
            System.Diagnostics.Debugger.Launch();
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
