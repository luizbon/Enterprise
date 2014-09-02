using System;
using System.Collections.Generic;
using System.Web;
using GridMvc;
using GridMvc.Sorting;
using PagedList;

namespace Enterprise.Core.Web.Grid
{
    public class PagedListGrid<T> : Grid<T> where T : class
    {
        private readonly IPagedList<T> _items;

        public PagedListGrid(IPagedList<T> items)
            : base(items)
        {
            _items = items;
            Pager = new PagedListGridPager(items);

            EnablePaging = true;
            Pager.PageSize = items.PageSize;
            Settings.SortSettings.ColumnName = HttpContext.Current.Request["sortColumn"];
            var direction = HttpContext.Current.Request["sortOrder"].IsEmpty("asc");
            Settings.SortSettings.Direction = direction == "asc"
                ? GridSortDirection.Ascending
                : GridSortDirection.Descending;
            EmptyGridText = Resources.Messages.NoRegistriesFound;
        }

        public override sealed IGridSettingsProvider Settings
        {
            get { return base.Settings; }
            set { base.Settings = value; }
        }

        protected override IEnumerable<T> GetItemsToDisplay()
        {
            return _items;
        }
    }
}
