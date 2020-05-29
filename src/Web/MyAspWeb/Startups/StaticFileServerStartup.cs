using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyAspWeb.Startups
{
    public class StaticFileServerStartup
    {
        public const string Share_Dir = @"wwwroot\shared";
        public string ShareDir { get; private set; }

        public StaticFileServerStartup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            ShareDir = Path.Combine(env.ContentRootPath, Share_Dir);
        }

        private IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            app.UseMiddleware(typeof(Middleware.RequestIPMiddleware));

            var shareDirs = Configuration.GetValue<string>("SharedDir").Split(';');
            string bdir = null;
            if (shareDirs != null)
            {
                foreach (var dir in shareDirs.ToList())
                {
                    if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
                        continue;
                    app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                    {
                        FileProvider = new PhysicalFileProvider(dir),
                    });
                    bdir = dir;
                    break;
                }
            }
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Add(".log", "txt/plain");
            if(string.IsNullOrEmpty(bdir) || !Directory.Exists(bdir))
            {
                bdir = ShareDir;
                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = new PhysicalFileProvider(ShareDir),
                });
            }
            var staticFile = new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(bdir),
                ContentTypeProvider = provider,
                //将不能识别的文件作为图片处理
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/x-msdownload",
            };
            //使用静态文件服务
            app.UseStaticFiles(staticFile);
        }
    }
}
