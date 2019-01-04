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
    public class HomeController : AdminController
    {
        private readonly IOrdersService ordersService;

        public HomeController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var orders = this.ordersService.GetAllOrders();

            return this.View(orders);
        }
    }
}
