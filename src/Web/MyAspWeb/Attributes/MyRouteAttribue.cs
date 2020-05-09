using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MyAspWeb.Attributes
{
    /// <summary>
    /// 自定义路由特性
    /// API/[controller]
    /// </summary>
    public class MyRouteAttribute : Attribute, IRouteTemplateProvider
    {
        public string Name => "API/[controller]";

        public int? Order { get; set; }

        public string Template { get; set; }
    }
}
