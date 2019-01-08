﻿using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICartService cartService;
        private readonly IMessageService messageService;
        private readonly IProductService productService;

        public HomeController(ICartService cartService, IMessageService messageService, IProductService productService)
        {
            this.cartService = cartService;
            this.messageService = messageService;
            this.productService = productService;
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

            var randomProducts = this.ViewData["RandomProducts"] = this.productService.GetRandomProducts(5);

            return this.View();
        }
        
        [HttpPost]
        public IActionResult Contacts(EmailViewModel model)
        {
            if (!ModelState.IsValid) return this.View(model);

            this.messageService.SendMessage(model);
            
            this.TempData["SendMessage"] = "Успешно изпратихте съобщение!";

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
