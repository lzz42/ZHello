using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace MyAspWeb.Startups
{
    public class StaticFileServerStartup : IStartup
    {
        public const string ShareDir = @"C:\IISRoot\WebSocket\tools";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory)
        {
            var dir = new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(ShareDir),
            };
            app.UseDirectoryBrowser(dir);

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Add(".log", "txt/plain");
            provider.Mappings[".myapp"] = "application/x-msdownload";
            provider.Mappings[".mp4"] = "application/x-msdownload";
            var staticFile = new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(ShareDir),
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
