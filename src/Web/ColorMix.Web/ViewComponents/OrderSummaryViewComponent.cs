using System.Linq;
using System.Security.Claims;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.ViewComponents
{
    public class OrderSummaryViewComponent : ViewComponent
    {
        private readonly ICartService cartService;
        private readonly UserManager<ColorMixUser> userManager;

        public OrderSummaryViewComponent(ICartService cartService, UserManager<ColorMixUser> userManager)
        {
            this.cartService = cartService;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.userManager.GetUserId((ClaimsPrincipal)this.User);

            var products = this.cartService.GetAllCartProducts(this.HttpContext.Session, userId);

            var totalPrice = products.Sum(x => x.Total);
            var tax = totalPrice * 0.2m;
            var priceWithoutTax = totalPrice - tax;

            this.ViewData["TotalPrice"] = $"{totalPrice:f2}";
            this.ViewData["Tax"] = $"{tax:f2}";
            this.ViewData["PriceWithoutTax"] = $"{priceWithoutTax:f2}";

            return this.View();
        }
    }
}
