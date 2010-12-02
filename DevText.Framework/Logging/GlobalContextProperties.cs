using System;
using Castle.Core.Logging;
using log4net;

namespace DevText.Framework.Logging
{
    public class GlobalContextProperties:IContextProperties
    {
        public object this[string key]
        {
            get
            {
                return GlobalContext.Properties[key];
            }
            set
            {
                GlobalContext.Properties[key] = value;
            }
        }
    }
}
