using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public partial class Service1 : ServiceBase
    {
        Logger logger;
        Watcher watcher;

        public Service1()
        {
            InitializeComponent();
            AutoLog = true;
            CanStop = true;
            watcher = new Watcher();
            logger = watcher.Logger;
           
        }

        protected override async void OnStart(string[] args)
        {
            await logger.RecordEntry("Cервис запущен");
            var thread = new Thread(new ThreadStart((watcher.Start)));
            thread.Start();
        }

        protected override async void OnStop()
        {
            await logger.RecordEntry("Cервис остановлен");
            watcher.Stop();
        }

       
    }
}
