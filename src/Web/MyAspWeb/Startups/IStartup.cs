using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyAspWeb.Startups
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
        //void Configure(IApplicationBuilder app, IWebHostEnvironment env);
        void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory);
    }
}
