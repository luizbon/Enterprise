using System.Linq;
using Enterprise.Core.Model;
using PagedList;

namespace Enterprise.Core.Service.Interfaces
{
    public interface IService<TEntity, in TKey> : ICRUDService<TEntity, TKey> where TEntity : Entity<TKey>
    {
        IPagedList<TEntity> GetByFilter(IPageFilter filter);
        IQueryable<TEntity> GetForReport(IPageFilter filter);
    }
}
