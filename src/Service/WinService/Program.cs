using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 服务进程的主线程
        static void Main()
        {
            ////服务的初始化不应超过30s
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WServiceTest()
            };
            ServiceBase.Run(ServicesToRun);

            //QuoteServer server = new QuoteServer();
            //server.OnEvent += Server_OnEvent;
            //server.Start(); 
            //Console.WriteLine("Wating For Client....");
            //Console.ReadKey();

        }


    }
}
