using System;
using System.Diagnostics;

namespace DevText.Framework.Events
{
    public class EventArgs<TValue>:EventArgs
    {
        public EventArgs(TValue value)
        {
            Value = value;
        }

        public TValue Value
        {
            get;
            private set;
        }
    }
}
