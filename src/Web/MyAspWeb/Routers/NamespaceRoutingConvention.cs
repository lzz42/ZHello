using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MyAspWeb.Routers
{
    public class NamespaceRoutingConvention : IControllerModelConvention
    {
        private readonly string BaseNamespace;
        public NamespaceRoutingConvention(string baseNamespace)
        {
            BaseNamespace = baseNamespace;
        }

        public void Apply(ControllerModel controller)
        {
            var hasRouteAttr = controller.Selectors.Any(s =>
            {
                return s.AttributeRouteModel != null;
            });
            if (hasRouteAttr)
                return;
            var ns = controller.ControllerType.Namespace;
            var template = new StringBuilder();
            template.Append(ns, BaseNamespace.Length + 1, ns.Length - BaseNamespace.Length - 1);
            template.Replace('.', '/');
            template.Append("[controller]");
            foreach (var item in controller.Selectors)
            {
                item.AttributeRouteModel = new AttributeRouteModel()
                {
                    Template = template.ToString(),
                };
            }
        }
    }
}
