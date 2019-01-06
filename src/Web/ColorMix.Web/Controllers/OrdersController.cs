using System;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ColorMix.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace ColorMix.Web.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IUserService userService;
        private readonly ICartService cartService;
        private readonly IOrdersService ordersService;
        private readonly UserManager<ColorMixUser> userManager;

        public OrdersController(IUserService userService,
                                ICartService cartService,
                                IOrdersService ordersService,
                                UserManager<ColorMixUser> userManager)
        {
            this.userService = userService;
            this.cartService = cartService;
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult CheckoutAddress()
        {
            var userId = this.userManager.GetUserId(this.User);

            var userData = userService.GetUserData(userId);

            var viewModel = new OrdersViewModel()
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Email = userData.Email,
                PhoneNumber = userData.PhoneNumber,
                AddressCity = userData.AddressCity,
                AddressCountry = userData.AddressCountry,
                AddressStreet = userData.AddressStreet,
                AddressZipCode = userData.AddressZipCode
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult OrderReview(OrdersViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("CheckoutAddress");

            var products = cartService.GetAllCartProducts(HttpContext.Session, User);

            model.Products = products.ToList();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PlaceOrder(OrdersViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("OrderReview");

            var products = cartService.GetAllCartProducts(HttpContext.Session, User);

            model.Products = products.ToList();

            ordersService.PlaceOrder(model, User);

            return RedirectToAction("MyOrders", "Users");
        }

        [Authorize]
        public IActionResult Details(Guid id)
        {
            var detailsModel = this.ordersService.GetOrderDetails(id);

            return this.View(detailsModel);
        }
    }
}
