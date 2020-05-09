using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace MyAspWeb.Routers
{
    public class MyMvcRouter : IRouter
    {
        private string[] urls { get; set; }

        public MyMvcRouter(IApplicationBuilder builder)
        {
            Builder = builder;
            urls = new string[]
            {
                "html"
            };
        }

        public IApplicationBuilder Builder { get; private set; }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {
            var requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');
            if (urls.Contains(requestedUrl, StringComparer.OrdinalIgnoreCase))
            {
                context.Handler = async ctx => {
                    var response = ctx.Response;
                    byte[] bytes = Encoding.ASCII.GetBytes($"This URL: {requestedUrl} is not available now");
                    await response.Body.WriteAsync(bytes, 0, bytes.Length);
                };
            }
            return Task.CompletedTask;
        }
    }
}
