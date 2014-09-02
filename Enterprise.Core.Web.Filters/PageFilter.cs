using System.ComponentModel.DataAnnotations;
using System.Linq;
using Enterprise.Core.Web.Filters.Interfaces;

namespace Enterprise.Core.Web.Filters
{
    public abstract class PageFilter : Service.Filters.PageFilter, IPageFilter
    {
        [ScaffoldColumn(false)]
        public override int? Page
        {
            get { return base.Page; }
            set { base.Page = value; }
        }

        [ScaffoldColumn(false)]
        public override int? PageSize
        {
            get { return base.PageSize; }
            set { base.PageSize = value; }
        }

        [ScaffoldColumn(false)]
        public override string SortColumn
        {
            get { return base.SortColumn; }
            set { base.SortColumn = value; }
        }

        [ScaffoldColumn(false)]
        public override string SortOrder
        {
            get { return base.SortOrder; }
            set { base.SortOrder = value; }
        }

        [ScaffoldColumn(false)]
        public bool Closed { get; set; }

        [ScaffoldColumn(false)]
        public virtual bool IsPartial { get; set; }

        [ScaffoldColumn(false)]
        public virtual bool Export { get; set; }

        public abstract bool CanExport();

        public abstract override IQueryable<T> Filter<T>(IQueryable<T> source);
    }
}