using System.ComponentModel.DataAnnotations;

namespace Enterprise.Core.Web.ViewModels
{
    public class EntityViewModel<TKey>
    {
        [ScaffoldColumn(false)]
        public TKey Id { get; set; }
    }
}