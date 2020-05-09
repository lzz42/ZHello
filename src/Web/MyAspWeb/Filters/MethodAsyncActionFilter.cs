using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyAspWeb.Filters
{
    public class MethodAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Before Excte "+ context.ActionDescriptor.DisplayName);
            await next();
            NLog.LogManager.GetCurrentClassLogger().Info("After Excte " + next.Method.Name);
        }
    }
}
