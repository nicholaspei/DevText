using System;
using System.Collections.Generic;

namespace DevText.Framework.Data
{
    public interface ISessionFactory
    {
        void CreateSessionFactory<T>();
    }
}
