using System;
using Castle.Core.Logging;

namespace DevText.Framework.Logging
{
    public class ThreadContextStack:IContextStack
    {
        #region Fields

        private log4net.Util.ThreadContextStack log4netStack;

        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.log4netStack.Count;
            }
        }
        #endregion

        #region Constructor
        public ThreadContextStack(log4net.Util.ThreadContextStack log4netStack)
        {
            this.log4netStack = log4netStack;
        }
        #endregion

        #region Methods

        public void Clear()
        {
            this.log4netStack.Clear();
        }

        public string Pop()
        {
            return this.log4netStack.Pop();
        }

        public IDisposable Push(string message)
        {
            return this.log4netStack.Push(message);
        }

        #endregion
    }
}
