using System;
using System.Web.Mvc;
using System.Web.Routing;

using MvcExtensions;
namespace DevText
{
    public class ConfigureRoutes : RegisterRoutesBase
    {
        public ConfigureRoutes(RouteCollection routes)
            : base(routes)
        {
        }

        protected override void Register()
        {

            Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}