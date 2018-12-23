using System;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using ColorMix.Services.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class CartController : BaseController
    {
        private const string ERROR = "Възникна грешка!";

        private readonly ICartService cartService;
        private readonly IProductService productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            this.cartService = cartService;
            this.productService = productService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var products = this.cartService.GetAllCartProducts(this.HttpContext.Session);

            return View(products);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(CartItemViewModel model)
        {
            this.cartService.AddToCart(model, this.HttpContext.Session);

            return this.RedirectToAction("Index");
        }

        public IActionResult Remove(Guid id)
        {
            if (!this.productService.CheckIfProductExists(id))
            {
                return View("Error", new ErrorViewModel() { Message = ERROR });
            }

            this.cartService.Remove(id, this.HttpContext.Session);

            return this.RedirectToAction("Index");
        }
    }
}
