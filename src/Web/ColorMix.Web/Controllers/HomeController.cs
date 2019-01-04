using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICartService cartService;

        public HomeController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.cartService.MoveFromSessionCartToDbCart(this.HttpContext.Session, this.User);
            }

            if (this.User.IsInRole("Admin"))
            {
                return this.RedirectToAction("Index", "Home", new {area = "Administration"});
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
