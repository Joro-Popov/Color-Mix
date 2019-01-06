using System.Linq;
using System.Threading.Tasks;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly IOrdersService ordersService;
        private readonly UserManager<ColorMixUser> userManager;

        public UsersController(IUserService userService, 
                               IOrdersService ordersService,
                               UserManager<ColorMixUser> userManager)
        {
            this.userService = userService;
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult MyOrders()
        {
            var orders = this.ordersService.GetUserOrders(this.User).ToList();

            return View(orders);
        }

        [Authorize]
        public IActionResult MyPersonalData()
        {
            var userId = this.userManager.GetUserId(this.User);

            var profileData = this.userService.GetUserData(userId);

            return View(profileData);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePersonalData(ProfileDataViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (ModelState.IsValid)
            {
                this.userService.ChangeUserData(user, model);

                await this.userManager.UpdateAsync(user);
            }

            return RedirectToAction("MyPersonalData");
        }
    }
}
