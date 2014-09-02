using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Enterprise.Core.Model;
using Enterprise.Core.Web.ViewModels;
using PagedList;

namespace Enterprise.Core.Web.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IPagedList<TViewModel> ToViewModel<TViewModel, TEntity, TKey>(this IPagedList<TEntity> entity)
            where TViewModel : EntityViewModel<TKey>
            where TEntity : Entity<TKey>
        {
            var list = entity.ToList();

            return new StaticPagedList<TViewModel>(list.ToViewModel<TViewModel, TEntity, TKey>(), entity);
        }

        public static TEntity ToEntity<TEntity, TKey>(this EntityViewModel<TKey> model) where TEntity : Entity<TKey>
        {
            return Mapper.Map<TEntity>(model);
        }

        public static TViewModel ToViewModel<TViewModel, TKey>(this Entity<TKey> entity)
            where TViewModel : EntityViewModel<TKey>
        {
            return Mapper.Map<TViewModel>(entity);
        }

        public static IEnumerable<TViewModel> ToViewModel<TViewModel, TEntity, TKey>(this IEnumerable<TEntity> entity)
            where TEntity : Entity<TKey>
        {
            return Mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(entity);
        }
    }
}