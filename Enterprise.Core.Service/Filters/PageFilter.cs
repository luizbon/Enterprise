using System;
using System.Linq;
using Enterprise.Core.Service.Interfaces;
using PagedList;

namespace Enterprise.Core.Service.Filters
{
    public abstract class PageFilter : IPageFilter
    {
        public virtual int? Page { get; set; }

        public virtual int? PageSize { get; set; }

        public virtual string SortColumn { get; set; }

        public virtual string SortOrder { get; set; }

        public virtual IQueryable<T> Sort<T>(IQueryable<T> source)
        {
            if (!SortColumn.IsEmpty())
            {
                source = source.OrderBy(SortColumn, SortOrder);
            }

            return source;
        }

        public virtual IPagedList<T> Paged<T>(IQueryable<T> source)
        {
            IPagedList<T> paged = source.ToPagedList(Page ?? 1, PageSize ?? 10);

            if (paged.PageCount < (Page ?? 0)) paged = source.ToPagedList(1, PageSize ?? 10);

            return paged;
        }

        public abstract IQueryable<T> Filter<T>(IQueryable<T> source);
    }
}