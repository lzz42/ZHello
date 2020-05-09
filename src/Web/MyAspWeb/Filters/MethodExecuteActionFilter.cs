using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyAspWeb.Filters
{
    public class MethodActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("OnActionExecuted");
            NLog.LogManager.GetCurrentClassLogger().Info(context.ActionDescriptor.DisplayName);
            NLog.LogManager.GetCurrentClassLogger().Info(context.ActionDescriptor.Id);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("OnActionExecuting");
            NLog.LogManager.GetCurrentClassLogger().Info(context.ActionDescriptor.DisplayName);
            NLog.LogManager.GetCurrentClassLogger().Info(context.ActionDescriptor.Id);
        }
    }
}
