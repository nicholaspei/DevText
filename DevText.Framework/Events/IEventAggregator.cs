using System;

namespace DevText.Framework.Events
{
     public interface IEventAggregator
    {
         TEvent GetEvent<TEvent>() where TEvent : EventBase;
    }
}
