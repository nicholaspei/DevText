using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DevText.Framework.Extensions;

namespace DevText.Framework.Events
{

    public class EventAggregator : Disposable, IEventAggregator
    {
        private readonly List<EventBase> events = new List<EventBase>();
        private readonly ReaderWriterLockSlim syncLock = new ReaderWriterLockSlim();

        public TEvent GetEvent<TEvent>() where TEvent : EventBase
        {
            TEvent eventInstance;

            using (syncLock.ReadAndMaybeWrite())
            {
                eventInstance = events.SingleOrDefault(evt => evt.GetType() == typeof(TEvent)) as TEvent;

                if (eventInstance == null)
                {
                    using (syncLock.Write())
                    {
                        eventInstance = Activator.CreateInstance<TEvent>();
                        events.Add(eventInstance);
                    }
                }
            }

            return eventInstance;
        }

        protected override void DisposeCore()
        {
            using (syncLock.Write())
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    events[i].Dispose();
                }

                events.Clear();
            }

            syncLock.Dispose();
        }
    }
}
