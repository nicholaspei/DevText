using System;
using NHibernate;

namespace DevText.Framework.Data
{
    public interface IUnitOfWork:IDisposable
    {
        ISession Session { get; }

        void Commit();
    }
}
