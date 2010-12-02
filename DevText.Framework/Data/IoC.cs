using System;
using Microsoft.Practices.ServiceLocation;

namespace DevText.Framework.Data
{
    public static class IoC
    {
        static IoC()
        {

        }
        public static TService GetInstance<TService>()
        {
            return ServiceLocator.Current.GetInstance<TService>();
        }

        public static object GetInstance(Type serviceType)
        {
            return ServiceLocator.Current.GetInstance(serviceType);
        }

    }
}
