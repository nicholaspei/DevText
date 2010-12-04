﻿using System;
using System.Linq;

namespace DevText.Framework.Events
{
    public abstract class EventBase<TPayload>:EventBase
    {
        public virtual SubscriptionToken Subscribe(Action<TPayload> action)
        {
            return Subscribe(action, false);
        }

        public virtual SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, keepSubscriberReferenceAlive, obj => true);
        }

        public virtual SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter)
        {
            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
            IDelegateReference filterReference = new DelegateReference(filter, keepSubscriberReferenceAlive);

            EventSubscription<TPayload> subscription = new EventSubscription<TPayload>(actionReference, filterReference);

            return base.Subscribe(subscription);
        }

        public virtual void Publish(TPayload payload)
        {
            base.Publish(payload);
        }

        public virtual void Unsubscribe(Action<TPayload> subscriber)
        {
            lock (Subscriptions)
            {
                IEventSubscription eventSubscription = Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);

                if (eventSubscription != null)
                {
                    Subscriptions.Remove(eventSubscription);
                }
            }
        }

        public virtual bool Contains(Action<TPayload> subscriber)
        {
            IEventSubscription eventSubscription;

            lock (Subscriptions)
            {
                eventSubscription = Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);
            }

            return eventSubscription != null;
        }
    }
}
