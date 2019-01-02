using System;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using ColorMix.Services.Models.Cart;
using ColorMix.Services.Models.Products;
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
        
        public IActionResult Index()
        {
            var products = this.cartService.GetAllCartProducts(this.HttpContext.Session, this.User);
            
            return View(products);
        }

        [HttpPost]
        public IActionResult AddToCart(DetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.cartService.AddToCart(model, this.HttpContext.Session, this.User);

                return this.RedirectToAction("Index");
            }
            
            ModelState.AddModelError(string.Empty, ERROR);

            return this.RedirectToAction("Details","Products");
        }

        public IActionResult Remove(Guid id, string size)
        {
            if (!this.productService.CheckIfProductExists(id))
            {
                return View("Error", new ErrorViewModel() { Message = ERROR });
            }

            this.cartService.Remove(id, size, this.HttpContext.Session, this.User);

            return this.RedirectToAction("Index");
        }
    }
}
