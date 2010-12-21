using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Post.Repository;
using Post.Model;

using MvcExtensions;

namespace DevText
{
    public class RegistorServices:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<post>().As<IPost>().InstancePerLifetimeScope();
        //    builder.RegisterType<postRepository>().As<IpostRepository>().InstancePerLifetimeScope();
        }
    }
}