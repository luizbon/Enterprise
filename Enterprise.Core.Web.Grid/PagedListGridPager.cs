using System;
using System.Globalization;
using System.Linq;
using System.Web;
using GridMvc.Pagination;
using PagedList;

namespace Enterprise.Core.Web.Grid
{
    public class PagedListGridPager : IGridPager
    {
        private readonly CustomQueryStringBuilder _queryBuilder;
        private readonly IPagedList _items;
        public const int DefaultMaxDisplayedPages = 5;
        public const string DefaultPageQueryParameter = "page";
        public const string DefaultPagerViewName = "_GridPager";
        private int _maxDisplayedPages;

        private PagedListGridPager(HttpContext context)
        {
            if (context == null)
                throw new Exception("No http context here!");
            _queryBuilder = new CustomQueryStringBuilder(HttpContext.Current.Request.QueryString);
            ParameterName = DefaultPageQueryParameter;
            TemplateName = DefaultPagerViewName;
            MaxDisplayedPages = MaxDisplayedPages;
        }

        public PagedListGridPager(IPagedList items)
            : this(items, HttpContext.Current)
        {
        }

        public PagedListGridPager(IPagedList items, HttpContext context)
            : this(context)
        {
            _items = items;
        }

        public int CurrentPage
        {
            get { return _items.PageNumber; }
        }

        public int PageSize
        {
            get { return _items.PageSize; }
            set { }
        }

        public string ParameterName { get; set; }

        public string TemplateName { get; private set; }

        public int PageCount
        {
            get { return _items.PageCount; }
        }

        public int MaxDisplayedPages
        {
            get { return _maxDisplayedPages == 0 ? DefaultMaxDisplayedPages : _maxDisplayedPages; }
            set { _maxDisplayedPages = value; }
        }

        public int StartDisplayedPage
        {
            get { return (CurrentPage - MaxDisplayedPages / 2) < 1 ? 1 : CurrentPage - MaxDisplayedPages / 2; }
        }

        public int EndDisplayedPage
        {
            get
            {
                return (CurrentPage + MaxDisplayedPages / 2) > PageCount
                    ? PageCount
                    : CurrentPage + MaxDisplayedPages / 2;
            }
        }

        public void Initialize<T>(IQueryable<T> items)
        {
        }

        public virtual string GetLinkForPage(int pageIndex)
        {
            return _queryBuilder.GetQueryStringWithParameter(ParameterName,
                pageIndex.ToString(CultureInfo.InvariantCulture));
        }
    }
}
