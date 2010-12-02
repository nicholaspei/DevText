using System;
using Castle.Core.Logging;
using log4net;

namespace DevText.Framework.Logging
{
    public class ThreadContextProperties:IContextProperties
    {
        public object this[string key]
        {
            get
            {
                return ThreadContext.Properties[key];
            }
            set
            {
                ThreadContext.Properties[key] = value;
            }
        }
    }
}
