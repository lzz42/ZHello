using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyAspWeb.Filters
{
    public class AddHeaderFilterAttribute : ResultFilterAttribute
    {
        private readonly string Name;
        private readonly string Value;

        public AddHeaderFilterAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(Name, new string[] { Value });
            base.OnResultExecuting(context);
        }
    }
}