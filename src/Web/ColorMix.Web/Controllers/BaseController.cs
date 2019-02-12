using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    [Route("[controller]/[action]")]
    public abstract class BaseController : Controller
    {
        protected const string ERROR = "Възникна грешка!";
    }
}
