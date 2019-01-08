using System;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using ColorMix.Services.Models.Cart;
using ColorMix.Services.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class CartController : BaseController
    {
        private const string ERROR = "Възникна грешка!";

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
            
            var totalPrice = products.Sum(x => x.Total);
            var tax = totalPrice * 0.2m;
            var priceWithoutTax = totalPrice - tax;

            this.ViewData["TotalPrice"] = $"{totalPrice:f2}";
            this.ViewData["Tax"] = $"{tax:f2}";
            this.ViewData["PriceWithoutTax"] = $"{priceWithoutTax:f2}";

            return View(products);
        }

        [HttpPost]
        public IActionResult AddToCart(DetailsViewModel model)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (ModelState.IsValid)
            {
                this.cartService.AddToCart(model, this.HttpContext.Session, userId);

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
