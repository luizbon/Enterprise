using System.Collections.Generic;
using System.Linq;
using Enterprise.Core.Model;

namespace Enterprise.Core.Storage
{
    public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        IQueryable<TEntity> Items { get; }
        TKey Add(TEntity entity);
        void BulkInsert(ICollection<TEntity> list);
        void Delete(TEntity entity);
        TEntity Get(TKey id);
        void Update(TEntity entity);
        void Commit();
        void ClearCache(TEntity entity);
    }
}