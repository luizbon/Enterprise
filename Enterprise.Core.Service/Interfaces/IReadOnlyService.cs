using System.Collections.Generic;
using Enterprise.Core.Model;

namespace Enterprise.Core.Service.Interfaces
{
    public interface IReadOnlyService<out TEntity, in TKey> where TEntity : Entity<TKey>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(TKey id);
    }
}
