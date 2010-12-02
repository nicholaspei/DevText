using NHibernate;

namespace DevText.Framework.Data
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ITransaction _transaction;

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            Session = _sessionFactory.OpenSession();
            _transaction = Session.BeginTransaction();
        }

        public ISession Session
        {
            get;
            private set;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            Session.Close();
            Session = null;
        }
    }
}
