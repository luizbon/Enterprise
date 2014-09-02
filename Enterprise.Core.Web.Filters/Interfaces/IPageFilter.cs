namespace Enterprise.Core.Web.Filters.Interfaces
{
    public interface IPageFilter: Service.Interfaces.IPageFilter
    {
        bool Export { get; set; }
        bool CanExport();
        bool Closed { get; set; }
        bool IsPartial { get; set; }
    }
}
