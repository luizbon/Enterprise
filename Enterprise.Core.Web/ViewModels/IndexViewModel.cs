using Enterprise.Core.Web.Filters.Interfaces;
using PagedList;

namespace Enterprise.Core.Web.ViewModels
{
    public class IndexViewModel<TViewModel, TKey> where TViewModel : EntityViewModel<TKey>
    {
        public IPageFilter Filter
        {
            get;
            set;
        }

        public IPagedList<TViewModel> Items
        {
            get;
            set;
        }
    }
}
