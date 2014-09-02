using Enterprise.Core.Model;

namespace Enterprise.Core.Service.Interfaces
{
    public interface ICRUDService<TEntity, in TKey> : IReadOnlyService<TEntity, TKey> where TEntity : Entity<TKey>
    {
        TEntity Add(TEntity entity);

        void Delete(TKey id);

        void Delete(TEntity entity);

        TEntity Update(TEntity entity);
    }
}