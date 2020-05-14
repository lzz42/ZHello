using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MyAspWeb.Middleware
{
    public class BasicUser
    {
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class BasicAuthenticateMiddleware
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// 固定的基本认证 验证读取 头部
        /// </summary>
        public const string AuthHeader = "Authorization";
        /// <summary>
        /// 固定的基本认证 失败回复 头部
        /// </summary>
        public const string WWWAuthHeader = "www-Authenticate";
        /// <summary>
        /// 固定的基本认证 失败回复 内容
        /// </summary>
        public const string WWWAuth_ReponseContent = "Basic realm=\"localghost\"";
        private BasicUser _user;
        public BasicAuthenticateMiddleware(RequestDelegate next, BasicUser user)
        {
            _next = next;
            _user = user;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var req = httpContext.Request;
            string auth = req.Headers[AuthHeader];
            if (auth == null)
            {
                return BasicAuth(httpContext);
            }
            string[] authPars = auth.Split(' ');
            if (authPars.Length != 2)
            {
                return BasicAuth(httpContext);
            }
            string bass64 = authPars[1];
            string authValue;
            try
            {
                byte[] bytes = Convert.FromBase64String(bass64);
                authValue = Encoding.ASCII.GetString(bytes);
            }
            catch (Exception ex)
            {
                authValue = null;
            }
            if (string.IsNullOrEmpty(authValue))
                return BasicAuth(httpContext);
            string user, psw;
            int splitIndex = authValue.IndexOf(':');
            if (splitIndex == -1)
            {
                user = authValue;
                psw = string.Empty;
            }
            else
            {
                user = authValue.Substring(0, splitIndex);
                psw = authValue.Substring(splitIndex + 1);
            }
            if (_user.User.Equals(user) && _user.Password.Equals(psw))
            {
                return _next(httpContext);
            }
            else
            {
                return BasicAuth(httpContext);
            }
        }

        public static Task BasicAuth(HttpContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Add(WWWAuthHeader, WWWAuth_ReponseContent);
            return Task.FromResult(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BasicAuthenticateMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthenticateMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticateMiddleware>();
        }
    }
}
