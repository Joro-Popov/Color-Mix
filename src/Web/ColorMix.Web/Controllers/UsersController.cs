using System.Linq;
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
