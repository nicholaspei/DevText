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
            Func<IRouteConstraint> idConstraint = () => new PositiveLongConstraint();
            Func<IRouteConstraint> pageConstraint = () => new PositiveIntegerConstraint(true);
            Func<IRouteConstraint> aliasConstraint = () => new RegexConstraint(@"^[a-zA-Z0-9]+$");

            //         Routes.Clear();

            // Turns off the unnecessary file exists check
            //       Routes.RouteExistingFiles = true;

            // Ignore axd files
            //            Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            

        }

    }
}