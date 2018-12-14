using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Data.Models;
using ColorMix.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UserManager<ColorMixUser> userManager;

        public HomeController(UserManager<ColorMixUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {

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
