using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.ViewComponents
{
    public class OrderSummaryViewComponent : ViewComponent
    {
        private readonly ICartService cartService;

        public OrderSummaryViewComponent(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public IViewComponentResult Invoke()
        {
            var products = this.cartService.GetAllCartProducts(this.HttpContext.Session, (ClaimsPrincipal)this.User);

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
