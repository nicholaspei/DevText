using System;
using Castle.Core.Logging;
using log4net;

namespace DevText.Framework.Logging
{
    public class ThreadContextStacks:IContextStacks
    {

        public IContextStack this[string key]
        {
            get
            {
                return new ThreadContextStack(ThreadContext.Stacks[key]);
            }
        }
    }
}
