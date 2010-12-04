using System;

namespace DevText.Framework.Events
{

    public interface IEventSubscription
    {
        SubscriptionToken SubscriptionToken
        {
            get;
            set;
        }

        Action<object[]> GetExecutionStrategy();
    }
}
