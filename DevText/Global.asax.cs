using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Web;
using FluentValidation;
using FluentValidation.Mvc;
using FluentValidation.Attributes;

using MvcExtensions.Autofac;

using DevText.Framework.Mvc;
using Post.Repository;
namespace DevText
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : AutofacMvcApplication, IContainerProviderAccessor
    {
// Provider that holds the application container.   
        static IContainerProvider _containerProvider;   
        
        // Instance property that will be used by Autofac HttpModules   
        // to resolve and inject dependencies.  
        public IContainerProvider ContainerProvider   {     get { return _containerProvider; }   } 

        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //    routes.MapRoute(
        //        "Default", // Route name
        //        "{controller}/{action}/{id}", // URL with parameters
        //        new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
        //    );

        //}
        
        //protected void Application_Start()
        //{
            
        //}

        public override void Init()
        {
            AreaRegistration.RegisterAllAreas();

            var builder = new ContainerBuilder();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new DevTextViewEngine());
            // Autofac registor Controllers
            Autofac.Integration.Web.Mvc.RegistrationExtensions.RegisterControllers(builder, Assembly.GetExecutingAssembly());

            Autofac.Integration.Web.Mvc.RegistrationExtensions.RegisterModelBinders(builder, Assembly.GetExecutingAssembly());

            _containerProvider = new ContainerProvider(builder.Build());

            // Set the controller facotry using the container provider
          //  ControllerBuilder.Current.SetControllerFactory(new Autofac.Integration.Web.Mvc.AutofacControllerFactory(ContainerProvider));


            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            ModelValidatorProviders.Providers.Add(
                new FluentValidationModelValidatorProvider(new AttributedValidatorFactory()));

            builder.RegisterType<postRepository>().As<IpostRepository>().InstancePerLifetimeScope();
           

        }
    }
}