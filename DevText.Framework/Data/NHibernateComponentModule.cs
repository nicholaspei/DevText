using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevText.Framework.Logging;
using Autofac;
using Autofac.Core;
using Castle.Core.Logging;

using DevText.Framework.Logging;

namespace DevText.Framework.Data
{
    public class NHibernateComponentModule:Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
           
            // registor session factory :) 
            builder.RegisterType<SessionFactory>().As<ISessionFactory>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency().ExternallyOwned();

            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>().InstancePerLifetimeScope();


            // call CreateLogger in response to the request for an ILogger implementation
            builder.Register((ctx, ps) => CreateLogger(ctx, ps)).As<ILogger>().InstancePerDependency();

        }


        private static ILogger CreateLogger(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            // return an ILogger in response to Resolve<ILogger>(componentTypeParameter)
            var loggerFactory = context.Resolve<ILoggerFactory>();
            var containingType = parameters.TypedAs<Type>();
            return loggerFactory.Create(containingType);
        }
    }
}
