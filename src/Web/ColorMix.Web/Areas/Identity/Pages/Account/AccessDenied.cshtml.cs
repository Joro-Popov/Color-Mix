using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColorMix.Web.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            this.Message = "Нямате достъп до избрания ресурс!";
        }
    }
}

