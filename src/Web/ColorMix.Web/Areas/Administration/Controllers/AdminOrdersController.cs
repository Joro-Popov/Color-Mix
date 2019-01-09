using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : AdminController
    {
        private const string ERROR = "Възникна грешка!";

        private readonly IOrdersService ordersService;

        public AdminOrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public IActionResult AllOrders()
        {
            var sendOrders = this.ordersService.GetAllSendOrders();

            return this.View(sendOrders);
        }

        public IActionResult SendOrder(Guid orderId)
        {
            if (!this.ordersService.OrderExists(orderId))
            {
                return View("Error", new ErrorViewModel() { Message = ERROR });
            }

            this.ordersService.SendOrder(orderId);

            return this.RedirectToAction("AllOrders");
        }
    }
}
