using Microsoft.AspNetCore.Builder;
using MyAspWeb.Middleware;

namespace MyAspWeb.Extensions
{
    public static class RequestIPExtensions
    {
        public static IApplicationBuilder UseRequstIP(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestIPMiddleware>();
        }
    }
}