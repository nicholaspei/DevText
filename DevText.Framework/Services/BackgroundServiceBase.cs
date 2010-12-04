using System;
using DevText.Framework.Events;

namespace DevText.Framework.Services
{

    public abstract class BackgroundServiceBase : IBackgroundService
    {
        protected BackgroundServiceBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public abstract string Name
        {
            get;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        protected IEventAggregator EventAggregator
        {
            get;
            private set;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                OnStart();
                IsRunning = true;
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                OnStop();
                IsRunning = false;
            }
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }
    }
}
