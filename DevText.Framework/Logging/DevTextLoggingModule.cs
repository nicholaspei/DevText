﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevText.Framework.Logging;
using Autofac;
using Autofac.Core;
using Castle.Core.Logging;


namespace DevText.Framework.Logging
{
    public class DevTextLoggingModule:Autofac.Module
    {
        protected override void Load(ContainerBuilder moduleBuilder)
        {
            moduleBuilder.RegisterType<Log4netFactory>().As<ILoggerFactory>().InstancePerLifetimeScope();
            // by default, use Castle's TraceSource based logger factory
            moduleBuilder.RegisterType<TraceLoggerFactory>().As<ILoggerFactory>().InstancePerLifetimeScope();

            // call CreateLogger in response to the request for an ILogger implementation
            moduleBuilder.Register((ctx, ps) => CreateLogger(ctx, ps)).As<ILogger>().InstancePerDependency();

        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            var implementationType = registration.Activator.LimitType;

            // build an array of actions on this type to assign loggers to member properties
            var injectors = BuildLoggerInjectors(implementationType).ToArray();

            // if there are no loger properties,there's no reason to hook the activated event
            if (!injectors.Any())
                return;

            // otherwise, when an instance of this component is activated, inject the loggers on the instance
            registration.Activated += (s, e) =>
            {
                foreach (var injector in injectors)
                    injector(e.Context, e.Instance);
            };

        }

        private IEnumerable<Action<IComponentContext, object>> BuildLoggerInjectors(Type componentType)
        {
            // Look for settable prperties of type "ILogger"
            var loggerProperties = componentType
                    .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => new
                    {
                        PropertyInfo = p,
                        p.PropertyType,
                        IndexParameters = p.GetIndexParameters(),
                        Accessors = p.GetAccessors(false)
                    })
                    .Where(x => x.PropertyType == typeof(ILogger))   // must be a logger
                    .Where(x => x.IndexParameters.Count() == 0)  // must not be an indexer
                    .Where(x => x.Accessors.Length != 1 || x.Accessors[0].ReflectedType == typeof(void));// must have get/set or only set

            // Return an array of actions that resolve a logger and assign the property
            foreach (var entry in loggerProperties)
            {
                var propertyInfo = entry.PropertyInfo;

                yield return (ctx, instance) =>
                {
                    var propertyValue = ctx.Resolve<ILogger>(new TypedParameter(typeof(Type), componentType));
                    propertyInfo.SetValue(instance, propertyValue, null);
                };
            }
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
