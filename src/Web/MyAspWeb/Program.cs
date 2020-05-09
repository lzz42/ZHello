using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAspWeb.Models;
using MyAspWeb.Startups;

namespace MyAspWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-3
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                var builler = CreateBuilder(args);
                if (builler != null)
                {
                    builler.Build().Run();
                }
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        private static IHostBuilder CreateBuilder(string[] args)
        {
            IHostBuilder builder = null;
            string v = "learn";
            switch (v)
            {
                case "static-file-server":
                    builder = CreateHostBulder_StaticFileServer(args);
                    break;
                case "learn":
                    builder = CreateHostBulder_Learn(args);
                    break;
                default:
                    break;
            }
            return builder;
        }

        private static IHostBuilder CreateHostBulder_StaticFileServer(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                     {
                         webBuilder.UseStartup<StaticFileServerStartup>();
                         webBuilder.UseUrls("https://*:8849");
                     });
        }

        private static IHostBuilder CreateHostBulder_Learn(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<LearnTestStartup>();
                        webBuilder.UseUrls("https://*:8850");
                        webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                        //webBuilder.UseContentRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
                    });
                    //.UseContentRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
        }
    }
}
