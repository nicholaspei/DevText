using System;
using System.Collections.Generic;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DevText.Framework.Data
{
    public class SessionFactory:ISessionFactory
    {
        private string _sqlconfigstring;
        
        public SessionFactory()
        {
            _sqlconfigstring = System.Configuration.ConfigurationManager.ConnectionStrings["Devtext"].ConnectionString;
        }

        public void CreateSessionFactory<T>()
        {
            IPersistenceConfigurer persistenceConfigurer;
            persistenceConfigurer =
            MsSqlConfiguration.MsSql2008.ConnectionString(c => c.Is(_sqlconfigstring));

            Fluently.Configure()
               .Database(persistenceConfigurer)
               .Mappings(m =>
                   m.FluentMappings.AddFromAssemblyOf<T>())
                        .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ReleaseConnections, "on_close"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(NHibernate.ByteCode.Castle.ProxyFactoryFactory).AssemblyQualifiedName))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, "create"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ShowSql, "true"))
               .ExposeConfiguration(BuildSchema)
               .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config).SetOutputFile(@"./Schema.sql").Create(true, true);
        }
    }
}
