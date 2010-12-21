using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcExtensions;

namespace DevText.Routes
{
    public partial class ConfigureRoutes : RegisterRoutesBase
    {
        public ConfigureRoutes(RouteCollection routes)
            : base(routes)
        {
        }
        protected override void Register()
        {

            Routes.MapRoute(
                "Post", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Post", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
    }
}