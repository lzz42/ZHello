using System;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace MyAspWeb.Attributes
{
    public class CompanyCodeAttribute : Attribute, IActionConstraint
    {
        public int Order
        {
            get
            {
                return 0;
            }
        }

        public bool Accept(ActionConstraintContext context)
        {
            if (context.RouteContext.RouteData.Values["CompanyCode"] == null)
                return false;
            var str = context.RouteContext.RouteData.Values["CompanyCode"].ToString();
            switch (str.ToUpper())
            {
                case "A":
                case "B":
                case "C":
                    return true;

                default:
                    break;
            }
            return false;
        }
    }
}