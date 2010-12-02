using System;
using Castle.Core.Logging;
using log4net;

namespace DevText.Framework.Logging
{
    public class ExtendedLog4netLogger:Log4netLogger,IExtendedLogger,ILogger
    {
        #region Fields

        private ExtendedLog4netFactory factory;
        private static readonly IContextProperties globalContextProperties = new GlobalContextProperties();
        private static readonly IContextProperties threadContextProperties = new ThreadContextProperties();
        private static readonly IContextStacks threadContextStacks = new ThreadContextStacks();
 
        #endregion

        #region Properties
        
        protected internal ExtendedLog4netFactory Factory
        {
            get
            {
                return this.factory;
            }
            set
            {
                this.factory = value;
            }
        }

        public IContextProperties GlobalProperties
        {
            get 
            {
                return globalContextProperties;
            }
        }

        public IContextProperties ThreadProperties
        {
            get
            {
                return threadContextProperties;
            }
        }

        public IContextStacks ThreadStacks
        {
            get
            {
                return threadContextStacks ;
            }
        }

        #endregion

        #region Constructor

        public ExtendedLog4netLogger(log4net.Core.ILogger logger, ExtendedLog4netFactory factory)
        {
            base.Logger = logger;
            this.Factory = factory;
        }

        public ExtendedLog4netLogger(ILog log, ExtendedLog4netFactory factory):
            this(log.Logger,factory)
        {}

        #endregion

        #region Methods

        public override ILogger CreateChildLogger(string name)
        {
            return this.CreateExtendChildLogger(name);
        }

        public IExtendedLogger CreateExtendChildLogger(string name)
        {
            return this.Factory.Create(base.Logger.Name + "." + name);
        }

        #endregion
    }
}
