using System;
using NHibernate;
namespace DevText.Framework.Data
{
    interface IUnitOfWork:IDisposable
    {
        ISession Session { get; }

        void Commit();
    }
}
