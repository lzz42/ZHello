using System;
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

        public static IApplicationBuilder UseBasicAuthenticateMiddleware(this IApplicationBuilder builder,BasicUser user)
        {
            if (user == null)
                throw new ArgumentException("Need Basic User");
            return builder.UseMiddleware<BasicAuthenticateMiddleware>(user);
        }
    }
}