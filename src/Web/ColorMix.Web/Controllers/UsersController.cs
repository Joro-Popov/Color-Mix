using ColorMix.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        public IActionResult MyOrders()
        {
            return View();
        }

        [Authorize]
        public IActionResult MyPersonalData()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePersonalData()
        {
            return RedirectToAction("MyPersonalData");
        }
    }
}
