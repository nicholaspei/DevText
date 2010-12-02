using System;
using Castle.Core.Logging;
using Castle.Core.Logging.Factories;
using log4net;
using log4net.Config;

namespace DevText.Framework.Logging
{
    public class ExtendedLog4netFactory:AbstractExtendedLoggerFactory
    {
        public ExtendedLog4netFactory():
            this("log4net.config")
        {}
        public ExtendedLog4netFactory(string configFile)
        {
            XmlConfigurator.ConfigureAndWatch(AbstractExtendedLoggerFactory.GetConfigFile(configFile));
        }

        public override IExtendedLogger Create(string name, LoggerLevel level)
        {
            throw new NotSupportedException("Logger levels cannot be set at runtime. Please review your configuration file.");
        }

        public override IExtendedLogger Create(string name)
        {
            return new ExtendedLog4netLogger(LogManager.GetLogger(name), this);
        }
    }
}
