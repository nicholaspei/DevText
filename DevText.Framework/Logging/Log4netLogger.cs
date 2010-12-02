using System;
using System.Globalization;
using log4net.Core;
using log4net;
using log4net.Util;

    namespace DevText.Framework.Logging
    {
    [Serializable]
    public class Log4netLogger:MarshalByRefObject,Castle.Core.Logging.ILogger
    {
    #region Fields

    private static Type declaringType = typeof(Log4netLogger);
    private Log4netFactory factory;
    private ILogger logger;
     private static readonly Castle.Core.Logging.ILogger _instance = new Log4netLogger();

    #endregion

    #region Properties
   
    public static Castle.Core.Logging.ILogger Instance
    {
        get { return _instance; }
    }

    protected internal Log4netFactory Factory 
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

    protected internal ILogger Logger
    {
        get
        {
            return this.logger;
        }
        set
        {
            this.logger = value;
        }
    }
    
        public bool IsDebugEnabled
    {
            get
        { 
            return this.Logger.IsEnabledFor(Level.Debug);
        }
    }

        public bool IsErrorEnabled
        {
            get
            {
                return this.Logger.IsEnabledFor(Level.Error);
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return this.Logger.IsEnabledFor(Level.Fatal);
            }
        }

        [Obsolete("Use IsFatalEnabled instead")]
        public bool IsFatalErrorEnabled
        {
            get
            {
                return this.Logger.IsEnabledFor(Level.Fatal);
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return this.Logger.IsEnabledFor(Level.Info);
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return this.Logger.IsEnabledFor(Level.Warn);
            }
        }

    #endregion

    #region Constructor

    internal Log4netLogger()
    { }
    public Log4netLogger(ILogger logger, Log4netFactory factory)
    {
        this.Logger = logger;
        this.Factory = factory;
    }

     internal Log4netLogger(ILog log, Log4netFactory factory):this(log.Logger,factory)
     {}
    #endregion

        #region Methods
     public virtual Castle.Core.Logging.ILogger CreateChildLogger(string name)
     {
         return this.Factory.Create(this.Logger.Name + "." + name);
     }

     public void Debug(string message)
     {
         if (this.IsDebugEnabled)
         {
             this.Logger.Log(declaringType, Level.Debug, message, null);
         }
     }

     [Obsolete("Use DebugFormat instead")]
     public void Debug(string format, params object[] args)
     {
         if (this.IsDebugEnabled)
         {
             this.Logger.Log(declaringType, Level.Debug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void Debug(string message, Exception exception)
     {
         if (this.IsDebugEnabled)
         {
             this.Logger.Log(declaringType, Level.Debug, message, exception);
         }
     }

     public void DebugFormat(string format, params object[] args)
     {
         if (this.IsDebugEnabled)
         {
             this.Logger.Log(declaringType, Level.Debug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void DebugFormat(Exception exception, string format, params object[] args)
     {
         if (this.IsDebugEnabled)
         {
             this.Logger.Log(declaringType, Level.Debug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
         }
     }

     public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsDebugEnabled)
         {
             this.Logger.Log(declaringType, Level.Debug, new SystemStringFormat(formatProvider, format, args), null);
         }
     }

     public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsDebugEnabled)
         {
             this.Logger.Log(declaringType, Level.Debug, new SystemStringFormat(formatProvider, format, args), exception);
         }
     }

     public void Error(string message)
     {
         if (this.IsErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Error, message, null);
         }
     }

     public void Error(string message, Exception exception)
     {
         if (this.IsErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Error, message, exception);
         }
     }

     [Obsolete("Use ErrorFormat instead")]
     public void Error(string format, params object[] args)
     {
         if (this.IsErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Error, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void ErrorFormat(string format, params object[] args)
     {
         if (this.IsErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Error, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void ErrorFormat(Exception exception, string format, params object[] args)
     {
         if (this.IsErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Error, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
         }
     }

     public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Error, new SystemStringFormat(formatProvider, format, args), null);
         }
     }

     public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Error, new SystemStringFormat(formatProvider, format, args), exception);
         }
     }

     public void Fatal(string message)
     {
         if (this.IsFatalEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, message, null);
         }
     }

     public void Fatal(string message, Exception exception)
     {
         if (this.IsFatalEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, message, exception);
         }
     }

     [Obsolete("Use FatalFormat instead")]
     public void Fatal(string format, params object[] args)
     {
         if (this.IsFatalEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     [Obsolete("Use Fatal instead")]
     public void FatalError(string message)
     {
         if (this.IsFatalErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, message, null);
         }
     }

     [Obsolete("Use FatalFormat instead")]
     public void FatalError(string format, params object[] args)
     {
         if (this.IsFatalErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     [Obsolete("Use Fatal instead")]
     public void FatalError(string message, Exception exception)
     {
         if (this.IsFatalErrorEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, message, exception);
         }
     }

     public void FatalFormat(string format, params object[] args)
     {
         if (this.IsFatalEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void FatalFormat(Exception exception, string format, params object[] args)
     {
         if (this.IsFatalEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
         }
     }

     public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsFatalEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, new SystemStringFormat(formatProvider, format, args), null);
         }
     }

     public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsFatalEnabled)
         {
             this.Logger.Log(declaringType, Level.Fatal, new SystemStringFormat(formatProvider, format, args), exception);
         }
     }

     public void Info(string message)
     {
         if (this.IsInfoEnabled)
         {
             this.Logger.Log(declaringType, Level.Info, message, null);
         }
     }

     [Obsolete("Use InfoFormat instead")]
     public void Info(string format, params object[] args)
     {
         if (this.IsInfoEnabled)
         {
             this.Logger.Log(declaringType, Level.Info, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void Info(string message, Exception exception)
     {
         if (this.IsInfoEnabled)
         {
             this.Logger.Log(declaringType, Level.Info, message, exception);
         }
     }

     public void InfoFormat(string format, params object[] args)
     {
         if (this.IsInfoEnabled)
         {
             this.Logger.Log(declaringType, Level.Info, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void InfoFormat(Exception exception, string format, params object[] args)
     {
         if (this.IsInfoEnabled)
         {
             this.Logger.Log(declaringType, Level.Info, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
         }
     }

     public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsInfoEnabled)
         {
             this.Logger.Log(declaringType, Level.Info, new SystemStringFormat(formatProvider, format, args), null);
         }
     }

     public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsInfoEnabled)
         {
             this.Logger.Log(declaringType, Level.Info, new SystemStringFormat(formatProvider, format, args), exception);
         }
     }

     public override string ToString()
     {
         return this.Logger.ToString();
     }

     public void Warn(string message)
     {
         if (this.IsWarnEnabled)
         {
             this.Logger.Log(declaringType, Level.Warn, message, null);
         }
     }

     [Obsolete("Use WarnFormat instead")]
     public void Warn(string format, params object[] args)
     {
         if (this.IsWarnEnabled)
         {
             this.Logger.Log(declaringType, Level.Warn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void Warn(string message, Exception exception)
     {
         if (this.IsWarnEnabled)
         {
             this.Logger.Log(declaringType, Level.Warn, message, exception);
         }
     }

     public void WarnFormat(string format, params object[] args)
     {
         if (this.IsWarnEnabled)
         {
             this.Logger.Log(declaringType, Level.Warn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
         }
     }

     public void WarnFormat(Exception exception, string format, params object[] args)
     {
         if (this.IsWarnEnabled)
         {
             this.Logger.Log(declaringType, Level.Warn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
         }
     }

     public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsWarnEnabled)
         {
             this.Logger.Log(declaringType, Level.Warn, new SystemStringFormat(formatProvider, format, args), null);
         }
     }

     public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
     {
         if (this.IsWarnEnabled)
         {
             this.Logger.Log(declaringType, Level.Warn, new SystemStringFormat(formatProvider, format, args), exception);
         }
     }

        #endregion

    }
    }
