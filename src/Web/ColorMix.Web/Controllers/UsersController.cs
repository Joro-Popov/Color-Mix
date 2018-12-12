using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class UsersController : BaseController
    {
        [Authorize]
        public IActionResult MyOrders()
        {
            return View();
        }

        [Authorize]
        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
