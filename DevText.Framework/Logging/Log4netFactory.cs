using System;
using log4net;
using log4net.Config;
using Castle.Core.Logging;

namespace DevText.Framework.Logging
{
    public class Log4netFactory :AbstractLoggerFactory
    {
        #region Methods
        public Log4netFactory()
            : this("log4net.config")
        { }

        public Log4netFactory(string configFile)
        {
            XmlConfigurator.ConfigureAndWatch(AbstractLoggerFactory.GetConfigFile(configFile));
        }

        public override ILogger Create(string name)
        {
            return new Log4netLogger(LogManager.GetLogger(name), this);
        }

        public override ILogger Create(string name, LoggerLevel level)
        {
            throw new NotSupportedException("Logger levels cannot be set at runtime. Please review your configuration file");
        }

        #endregion
    }
}
