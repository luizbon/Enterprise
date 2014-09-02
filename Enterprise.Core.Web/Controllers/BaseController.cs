using System.Web.Mvc;
using Enterprise.Core.Web.Helpers;

namespace Enterprise.Core.Web.Controllers
{
    public abstract class BaseController: Controller
    {
        protected FlashHelper Flash { get; set; }

        protected BaseController()
        {
            Flash = new FlashHelper(TempData);
        }
    }
}
