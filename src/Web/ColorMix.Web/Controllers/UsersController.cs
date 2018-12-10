using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class UsersController : BaseController
    {
        public IActionResult MyOrders()
        {
            return this.View();
        }

        public IActionResult MyProfile()
        {
            return this.View();
        }
    }
}
