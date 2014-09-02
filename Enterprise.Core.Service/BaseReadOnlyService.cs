using System.Collections.Generic;
using Enterprise.Core.Model;
using Enterprise.Core.Service.Interfaces;
using Enterprise.Core.Storage;

namespace Enterprise.Core.Service
{
    public class BaseReadOnlyService<TEntity, TKey> : IReadOnlyService<TEntity, TKey> where TEntity : Entity<TKey>
    {
        protected readonly IRepository<TEntity, TKey> Repository;

        public BaseReadOnlyService(IRepository<TEntity, TKey> repository)
        {
            Repository = repository;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            IEnumerable<TEntity> list = Repository.Items;
            return list;
        }

        public virtual TEntity Get(TKey id)
        {
            TEntity tEntity = Repository.Get(id);
            return tEntity;
        }
    }
}