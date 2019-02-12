using System;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using ColorMix.Services.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartService cartService;
        private readonly IProductService productService;
        private readonly UserManager<ColorMixUser> userManager;

        public CartController(ICartService cartService, IProductService productService, UserManager<ColorMixUser> userManager)
        {
            this.cartService = cartService;
            this.productService = productService;
            this.userManager = userManager;
        }
        
        public IActionResult Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var products = this.cartService.GetAllCartProducts(this.HttpContext.Session, userId);

            return this.View(products);
        }

        [HttpPost]
        public IActionResult AddToCart(DetailsViewModel model)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, ERROR);

                return this.RedirectToAction("Details", "Products");
            }

            this.cartService.AddToCart(model, this.HttpContext.Session, userId);

            return this.RedirectToAction("Index");
        }

        public IActionResult Remove(Guid id)
        {
            if (!this.productService.CheckIfProductExists(id))
            {
                return View("Error", new ErrorViewModel() { Message = ERROR });
            }
            
            this.cartService.Remove(id, this.HttpContext.Session, this.User);
            
            return this.RedirectToAction("Index");
        }
    }
}
