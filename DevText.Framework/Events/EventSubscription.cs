using System;
using System.Diagnostics;
using Castle.Core.Logging;
using DevText.Framework.Logging;

namespace DevText.Framework.Events
{
    public class EventSubscription<TPayload> : IEventSubscription
    {
        private readonly IDelegateReference actionReference;
        private readonly IDelegateReference filterReference;

        private ILogger _logger;

        public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
        {
            _logger = Log4netLogger.Instance;

            if (!(actionReference.Target is Action<TPayload>))
            {
                _logger.Error("action reference argument exception");
                 
            }

            if (!(filterReference.Target is Predicate<TPayload>))
            {
                _logger.Error("filter referejce argument exception");
            }

            this.actionReference = actionReference;
            this.filterReference = filterReference;
        }

        public Action<TPayload> Action
        {
            get
            {
                return (Action<TPayload>)actionReference.Target;
            }
        }

        public Predicate<TPayload> Filter
        {
            get
            {
                return (Predicate<TPayload>)filterReference.Target;
            }
        }

        public SubscriptionToken SubscriptionToken
        {
            get;
            set;
        }


        public Action<object[]> GetExecutionStrategy()
        {
            Action<TPayload> action = Action;
            Predicate<TPayload> filter = Filter;

            if (action != null && filter != null)
            {
                return arguments =>
                {
                    TPayload argument = default(TPayload);

                    if (arguments != null && arguments.Length > 0 && arguments[0] != null)
                    {
                        argument = (TPayload)arguments[0];
                    }

                    if (filter(argument))
                    {
                        InvokeAction(action, argument);
                    }
                };
            }

            return null;
        }
        protected virtual void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            action(argument);
        }
    }
}
