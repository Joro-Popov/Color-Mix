﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Controllers
{
    [Route("[controller]/[action]")]
    public abstract class BaseController : Controller
    {
    }
}
