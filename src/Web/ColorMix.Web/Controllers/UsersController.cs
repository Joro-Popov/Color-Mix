using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    public class UsersController : BaseController
    {
        [Authorize]
        public IActionResult MyOrders()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult MyProfile()
        {
            return this.View();
        }
    }
}
