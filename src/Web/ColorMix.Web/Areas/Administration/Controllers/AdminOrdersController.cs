using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : AdminController
    {
        private readonly IOrdersService ordersService;

        public AdminOrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }
        public IActionResult SendOrders()
        {
            var sendOrders = this.ordersService.GetAllSendOrders();

            return this.View(sendOrders);
        }
    }
}
