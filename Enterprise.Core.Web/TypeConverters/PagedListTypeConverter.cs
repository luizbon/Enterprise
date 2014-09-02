using System.Linq;
using AutoMapper;
using Enterprise.Core.Model;
using Enterprise.Core.Web.Extensions;
using PagedList;

namespace Enterprise.Core.Web.TypeConverters
{
    public class PagedListTypeConverter<TEntity, TViewModel, TKey> : ITypeConverter<IPagedList<TEntity>, IPagedList<TViewModel>>
        where TEntity : Entity<TKey>
    {
        public IPagedList<TViewModel> Convert(ResolutionContext context)
        {
            var source = (PagedList<TEntity>)context.SourceValue;
            var list = source.ToList().ToViewModel<TViewModel, TEntity, TKey>();
            return new StaticPagedList<TViewModel>(list, source.PageNumber, source.PageSize, source.TotalItemCount);
        }
    }
}
