using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ColorMix.Web.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IUserService userService;
        private readonly ICartService cartService;
        private readonly IOrdersService ordersService;

        public OrdersController(IUserService userService,
                                ICartService cartService,
                                IOrdersService ordersService)
        {
            this.userService = userService;
            this.cartService = cartService;
            this.ordersService = ordersService;
        }

        [Authorize]
        public IActionResult CheckoutAddress()
        {
            var userData = userService.GetUserData(User);

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
    }
}
