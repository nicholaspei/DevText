using System;

namespace DevText.Framework.Services
{
    public interface IBackgroundService
    {
        string Name
        {
            get;
        }
        bool IsRunning
        {
            get;
        }

        void Start();

        void Stop();

    }
}
