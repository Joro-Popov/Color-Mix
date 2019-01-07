using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Administration;
using ColorMix.Services.Models.Users;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ColorMix.Services.DataServices
{
    public class MessageService : IMessageService
    {
        private readonly ColorMixContext dbContext;
        private readonly IConfiguration configuration;

        public MessageService(ColorMixContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public void SendMessage(EmailViewModel model)
        {
            var message = new Message()
            {
                Content = model.Message,
                EmailAddress = model.EmailAddress,
                SendOn = DateTime.UtcNow,
                Title = model.Title
            };

            this.dbContext.Messages.Add(message);
            this.dbContext.SaveChanges();
        }

        public bool MessageExists(Guid messageId)
        {
            return this.dbContext.Messages.Any(x => x.Id == messageId);
        }

        public void SendAnswer(SendMessageViewModel model)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Color Mix", "popov937@abv.bg"));
            message.To.Add(new MailboxAddress(model.EmailAddress));
            message.Subject = model.Subject;
            message.Body = new TextPart("plain")
            {
                Text = model.Answer
            };

            var username = this.configuration["Authentication:Abv:Username"];
            var password = this.configuration["Authentication:Abv:Password"];

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.abv.bg", 465);
                client.Authenticate(username, password);
                client.Send(message);
                client.Disconnect(true);
            }
            
            this.ChangeMessageStatus(model.Id);
        }

        public MessageDetailsViewModel GetMessageDetails(Guid messageId)
        {
            var details = this.dbContext.Messages
                .Where(x => x.Id == messageId)
                .To<MessageDetailsViewModel>()
                .First();

            return details;
        }

        public IEnumerable<MessageViewModel> GetAllUnAnsweredMessages()
        {
            var unAnswered = this.dbContext.Messages
                .Where(x => x.IsAnswered == false)
                .To<MessageViewModel>()
                .ToList();

            return unAnswered;
        }

        private void ChangeMessageStatus(Guid messageId)
        {
            var message = this.dbContext.Messages
                .FirstOrDefault(x => x.Id == messageId);

            message.IsAnswered = true;
            
            this.dbContext.Messages.Update(message);
            this.dbContext.SaveChanges();
        }
    }
}
