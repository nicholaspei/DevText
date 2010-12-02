using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevText.Framework.Data;

namespace DevText.Framework.Data
{
    public interface IRepository<TEntity> where TEntity:IEntity
    {
        IUnitOfWork UnitOfWork { get; set; }

        IEnumerable<TEntity> All();

        int Add(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> condition);
    }
}
