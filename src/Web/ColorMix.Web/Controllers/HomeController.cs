using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UserManager<ColorMixUser> userManager;
        private readonly ICartService cartService;

        public HomeController(UserManager<ColorMixUser> userManager, ICartService cartService)
        {
            this.userManager = userManager;
            this.cartService = cartService;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.cartService.MoveFromSessionCartToDbCart(this.HttpContext.Session, this.User);
            }

            return this.View();
        }

        public IActionResult Contacts()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
