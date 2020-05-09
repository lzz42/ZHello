using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WService
{
    public partial class WServiceTest : ServiceBase
    {
        public WServiceTest()
        {
            InitializeComponent();
        }

        protected override void OnPause()
        {
            //base.OnPause();
            mQuoteServer.Suspend();
        }

        protected override void OnShutdown()
        {
            mQuoteServer.Stop();
            //base.OnShutdown();
        }

        protected override void OnContinue()
        {
            mQuoteServer.Start();
            //base.OnContinue();
        }

        private QuoteServer mQuoteServer;

        protected override void OnStart(string[] args)
        {
            mQuoteServer = new QuoteServer();
            mQuoteServer.Start();
        }


        protected override void OnStop()
        {
            mQuoteServer.Stop();
        }

        public const int cmdRefresh = 128;

        protected override void OnCustomCommand(int command)
        {
            //base.OnCustomCommand(command);
            switch (command)
            {
                case cmdRefresh:
                    mQuoteServer?.RefreshQuotes();
                    break;
                default:
                    break;
                    
            }
        }



    }

    public class ServiceControllerEx : ServiceController
    {
        
    }

    public class ServiceProcessInstallerEx : ServiceProcessInstaller
    {
        
    }

    public class ServiceInstallerEx : ServiceInstaller
    {
        
    }


}
