using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using MyAspWeb.Contexts;
using MyAspWeb.Extensions;
using MyAspWeb.Modules;

namespace MyAspWeb.Startups
{
    public class LearnTestStartup
    {
        private ConfigurationBuilder Builder { get; set; }

        private IConfigurationRoot Configuration { get; set; }

        public LearnTestStartup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("gconfig.json");
            Configuration = builder.Build();
        }

        private void GetSetConfig()
        {
            //获取和设置配置
            Builder = new ConfigurationBuilder();
            Builder.AddInMemoryCollection();
            var config = Builder.Build();
            //设置
            config["key1"] = "value1";
            //取值
            var setting = config["key1"];
            //使用内置数据源
            var jsonConfig = Builder.SetBasePath(Directory.GetCurrentDirectory())
                                    .AddInMemoryCollection(new Dictionary<string, string>()
                                    {
                                        {"username","Guest" },
                                    })
                                    .AddJsonFile("appsettings.json", false, true)
                                    .AddJsonFile("appsettings.dev.json", false, true);
            //使用选项和配置对象
        }

        #region 使用Autofac 代替默认的DI

        public IServiceProvider ConfigureUseAutofacDIServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(ops =>
            {
                ops.EnableEndpointRouting = false;
            });
            var builder = new ContainerBuilder();
            builder.RegisterModule<DefaultModule>();
            var c = builder.Build();
            return new AutofacServiceProvider(c);
        }

        public void ConfigureUseAutofacDI(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory)
        {
        }

        #endregion 使用Autofac 代替默认的DI

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<Models.GameSetting>(Configuration);
            services.Configure<MvcOptions>(ops =>
            {
                ops.EnableEndpointRouting = false;
            });
            //添加框架服务
            services.AddDbContext<BloggingDbContext>(ops =>
            {
                ops.UseSqlite("Data Source=./blog.db");
            }, ServiceLifetime.Singleton);
            services.AddControllersWithViews();
            //添加程序服务
            //services.AddTransient<Controllers.ICharacterRepository, Controllers.CharacterRepository>();
            //services.AddScoped<Controllers.ICharacterRepository, Controllers.CharacterRepository>();
            //services.AddSingleton<Repositories.IBlogRepository, Repositories.BlogRepository>();
            //配置Session
            services.AddDistributedMemoryCache();
            services.AddSession();
            //使用caching
            services.AddMemoryCache();
            services.AddMvc(options =>
            {
                options.MaxModelValidationErrors = 80;
                //通过类型添加过滤器，每次请求都会创建一个实例
                options.Filters.Add(typeof(Filters.MethodActionFilter));
                //通过实例添加过滤器，该实例用于处理每一个请求
                options.Filters.Add(new Filters.MethodAsyncActionFilter());
                options.Filters.Add(new Filters.AddHeaderFilterAttribute("sys01","ok01"));
            });

            services.AddTransient<Repositories.IBlogRepository, Repositories.BlogRepository>();
            services.AddTransient<Services.ISystemTime, Services.SystemService>();
            services.AddTransient<Services.BlogStatisticsService>();
            services.AddTransient(p => new Filters.AddHeaderFilterAttribute("sys02", "ok02"));
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory)
        {
            //解决日志显示中文乱码问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            var dFiles = new DefaultFilesOptions();
            dFiles.DefaultFileNames.Clear();
            dFiles.DefaultFileNames.Add("root/Index.html");
            app.UseDefaultFiles(dFiles);
            //app.UseRouting();
            //使用自定义中间件服务
            app.UseRequstIP();
            app.UseMvcWithDefaultRoute();
            //设置路由中间件
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "blog",
                    template: "blog/{action=Index}");
                routes.MapRoute(
                    name: "html",
                    template: "html",
                    "~/wwwroot/WebSocketClient.html");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults:"Home/Home.html");
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {  
                OnPrepareResponse = (response) =>
                {
                    response.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=2592000";
                }
            });
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}