using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using DevText.Framework.Extensions;

namespace DevText.Framework.Events
{
    public abstract class EventBase:Disposable
    {
        private readonly List<IEventSubscription> subscriptions = new List<IEventSubscription>();
        private readonly ReaderWriterLockSlim syncLock = new ReaderWriterLockSlim();

        protected ICollection<IEventSubscription> Subscriptions
        {
            get
            {
                return subscriptions;
            }
        }


        public virtual void Unsubscribe(SubscriptionToken token)
        {
            using (syncLock.ReadAndMaybeWrite())
            {
                IEventSubscription subscription = subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);

                if (subscription != null)
                {
                    using (syncLock.Write())
                    {
                        subscriptions.Remove(subscription);
                    }
                }
            }
        }

        public virtual bool Contains(SubscriptionToken token)
        {
            using (syncLock.Read())
            {
                return subscriptions.Any(evt => evt.SubscriptionToken == token);
            }
        }

        protected virtual void Publish(params object[] arguments)
        {
            IEnumerable<Action<object[]>> executionStrategies = PruneAndReturnStrategies();

            foreach (Action<object[]> executionStrategy in executionStrategies)
            {
                executionStrategy(arguments);
            }
        }

        protected virtual SubscriptionToken Subscribe(IEventSubscription eventSubscription)
        {
            eventSubscription.SubscriptionToken = new SubscriptionToken();

            using (syncLock.Write())
            {
                subscriptions.Add(eventSubscription);
            }

            return eventSubscription.SubscriptionToken;
        }

        protected override void DisposeCore()
        {
            using (syncLock.Write())
            {
                subscriptions.Clear();
            }

            syncLock.Dispose();
        }

        private IEnumerable<Action<object[]>> PruneAndReturnStrategies()
        {
            List<Action<object[]>> returnList = new List<Action<object[]>>();

            using (syncLock.ReadAndMaybeWrite())
            {
                for (int i = subscriptions.Count - 1; i >= 0; i--)
                {
                    Action<object[]> subscriptionAction = subscriptions[i].GetExecutionStrategy();

                    if (subscriptionAction == null)
                    {
                        using (syncLock.Write())
                        {
                            subscriptions.RemoveAt(i);
                        }
                    }
                    else
                    {
                        returnList.Add(subscriptionAction);
                    }
                }
            }

            return returnList;
        }
    }
}
