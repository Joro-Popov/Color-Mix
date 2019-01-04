using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly IOrdersService ordersService;

        public UsersController(IUserService userService, IOrdersService ordersService)
        {
            this.userService = userService;
            this.ordersService = ordersService;
        }

        [Authorize]
        public IActionResult MyOrders()
        {
            var orders = this.ordersService.GetUserOrders(this.User);

            return View(orders);
        }

        [Authorize]
        public IActionResult MyPersonalData()
        {
            var profileData = this.userService.GetUserData(this.User);

            return View(profileData);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePersonalData(ProfileDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                await this.userService.ChangeUserData(this.User, model);
            }

            return RedirectToAction("MyPersonalData");
        }
    }
}
