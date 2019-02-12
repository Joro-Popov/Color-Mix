using System.Linq;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly IOrdersService ordersService;
        private readonly UserManager<ColorMixUser> userManager;
        
        public UsersController(IUserService userService, 
                               IOrdersService ordersService, UserManager<ColorMixUser> userManager)
        {
            this.userService = userService;
            this.ordersService = ordersService;
            this.userManager = userManager;
        }
        
        public IActionResult MyOrders()
        {
            var userId = this.userManager.GetUserId(this.User);

            var orders = this.ordersService.GetUserOrders(userId).ToList();

            return View(orders);
        }
        
        public IActionResult MyPersonalData()
        {
            var userId = this.userManager.GetUserId(this.User);

            var profileData = this.userService.GetUserData(userId);

            return View(profileData);
        }

        [HttpPost]
        public IActionResult ChangePersonalData(ProfileDataViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("MyPersonalData");

            var userId = this.userManager.GetUserId(this.User);

            this.userService.ChangeUserData(userId, model);

            return RedirectToAction("MyPersonalData");
        }
    }
}
