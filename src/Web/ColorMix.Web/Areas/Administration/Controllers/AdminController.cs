using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Areas.Administration.Controllers
{
    public abstract class AdminController : Controller
    {
        protected const string ERROR = "Възникна грешка!";
    }
}
