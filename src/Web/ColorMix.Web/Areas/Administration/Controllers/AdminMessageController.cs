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
    public class AdminMessageController : AdminController
    {
        private readonly IMessageService messageService;

        public AdminMessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public IActionResult UnAnsweredMessages()
        {
            var unAnsweredMessages = this.messageService.GetAllUnAnsweredMessages();

            return this.View(unAnsweredMessages);
        }
    }
}
