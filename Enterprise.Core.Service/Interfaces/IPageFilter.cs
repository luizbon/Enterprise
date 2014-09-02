using System.Linq;
using Core.Service.Interfaces;
using PagedList;

namespace Enterprise.Core.Service.Interfaces
{
    public interface IPageFilter: IFilter
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
        string SortColumn { get; set; }
        string SortOrder { get; set; }
        IQueryable<T> Filter<T>(IQueryable<T> source);
        IQueryable<T> Sort<T>(IQueryable<T> source);
        IPagedList<T> Paged<T>(IQueryable<T> source);
    }
}
