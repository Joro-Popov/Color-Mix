using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Users;

namespace ColorMix.Services.DataServices
{
    public class MessageService : IMessageService
    {
        private readonly ColorMixContext dbContext;

        public MessageService(ColorMixContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SendMessage(EmailViewModel model)
        {
            var message = new Message()
            {
                Content = model.Message,
                EmailAddress = model.EmailAddress,
                IsAnswered = false,
                SendOn = DateTime.UtcNow,
                Title = model.Title
            };

            this.dbContext.Messages.Add(message);
            this.dbContext.SaveChanges();
        }
    }
}
