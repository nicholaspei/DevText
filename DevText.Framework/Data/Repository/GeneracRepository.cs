using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace DevText.Framework.Data
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected GenericRepository()
            : this(IoC.GetInstance<IUnitOfWork>())
        {
        }

        protected GenericRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; set; }

        public IEnumerable<TEntity> All()
        {
            
            return UnitOfWork.Session.QueryOver<TEntity>().List();
        }

        public int Add(TEntity entity)
        {
            return (int)UnitOfWork.Session.Save(entity);
        }

        public void Remove(TEntity entity)
        {
            UnitOfWork.Session.Delete(entity);
        }

        public void Update(TEntity entity)
        {
            UnitOfWork.Session.Update(entity);
        }

        public IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> condition)
        {
            return UnitOfWork.Session.QueryOver<TEntity>()
                    .Where(condition)
                    .List();
        }
    }
}
