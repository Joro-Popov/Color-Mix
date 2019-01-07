using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using ColorMix.Services.Models.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class AdminMessageController : AdminController
    {
        private const string ERROR = "Възникна грешка!";

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

        public IActionResult MessageDetails(Guid messageId)
        {
            if (!this.messageService.MessageExists(messageId))
            {
                return View("Error", new ErrorViewModel() { Message = ERROR });
            }

            var details = this.messageService.GetMessageDetails(messageId);

            return this.View(details);
        }

        [HttpPost]
        public IActionResult SendAnswer(SendMessageViewModel model)
        {
            if(!ModelState.IsValid) return this.RedirectToAction("MessageDetails", new { model.Id });

            this.messageService.SendAnswer(model);

            return this.RedirectToAction("UnAnsweredMessages");
        }
    }
}
