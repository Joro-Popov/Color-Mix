using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Categories;
using ColorMix.Services.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ColorMix.Services.DataServices.Tests
{
    public class MessageServiceTests
    {
        private readonly ColorMixContext dbContext;
        private readonly MessageService messageService;

        public MessageServiceTests()
        {
            this.dbContext = new ColorMixContext(new DbContextOptionsBuilder<ColorMixContext>()
                .UseInMemoryDatabase("ColorMix")
                .Options);

            var configuration = new Mock<IConfigurationRoot>();
            configuration.SetupGet(x => x[It.IsAny<string>()]).Returns("");

            this.messageService = new MessageService(this.dbContext, configuration.Object);

            Mapper.Reset();
            
            AutoMapperConfig.RegisterMappings(
                typeof(CategoryViewModel).Assembly
            );
        }

        [Fact]
        public void SendMessageShouldAddMessageToDatabase()
        {
            foreach (var entity in dbContext.Messages)
                dbContext.Messages.Remove(entity);

            var model = new EmailViewModel()
            {
                EmailAddress = "Some@EmailAddress.com",
                Message = "Some message",
                Title = "Some Title",
            };

            this.messageService.SendMessage(model);

            Assert.Equal(1, this.dbContext.Messages.Count());
        }

        [Fact]
        public void DeleteMessageShouldRemoveEntityFromDatabase()
        {
            foreach (var entity in dbContext.Messages)
                dbContext.Messages.Remove(entity);

            var messageId = Guid.NewGuid();

            var message = new Message()
            {
                Id = messageId,
                EmailAddress = "Some@email.com",
                Title = "SomeTitle",
                IsAnswered = false,
                Content = "Content",
                SendOn = DateTime.UtcNow
            };

            this.dbContext.Messages.Add(message);
            this.dbContext.SaveChanges();

            this.messageService.DeleteMessage(messageId);

            Assert.Empty(this.dbContext.Messages);
        }

        [Fact]
        public void GetMessageDetailsShouldNotReturnNull()
        {
            var messageId = Guid.NewGuid();
            
            var message = new Message()
            {
                Id = messageId,
                EmailAddress = "Some@email.com",
                Title = "SomeTitle",
                IsAnswered = false,
                Content = "Content",
                SendOn = DateTime.UtcNow
            };

            this.dbContext.Messages.Add(message);
            this.dbContext.SaveChanges();

            var actualMessage = this.messageService.GetMessageDetails(messageId);

            Assert.NotNull(actualMessage);
        }

        //[Fact]
        //public void GetAllUnansweredMessagesShouldReturnAllUnAnsweredMessages()
        //{
        //    foreach (var entity in dbContext.Messages)
        //        dbContext.Messages.Remove(entity);

        //    this.dbContext.SaveChanges();

        //    var messages = new List<Message>();

        //    for (int i = 0; i < 4; i++)
        //    {
        //        messages.Add(new Message()
        //        {
        //            Id = Guid.NewGuid(),
        //            EmailAddress = "Some@email.com",
        //            Title = "SomeTitle",
        //            IsAnswered = false,
        //            Content = "Content of the message",
        //            SendOn = DateTime.UtcNow
        //        });
        //    }

        //    messages.Add(new Message()
        //    {
        //        Id = Guid.NewGuid(),
        //        EmailAddress = "Some@email.com",
        //        Title = "SomeTitle",
        //        IsAnswered = true,
        //        Content = "Content of the message",
        //        SendOn = DateTime.UtcNow
        //    });
            
        //    this.dbContext.Messages.AddRange(messages);
        //    this.dbContext.SaveChanges();

        //    var expectedMessages = this.messageService.GetAllUnAnsweredMessages();

        //    Assert.Equal(4, expectedMessages.Count());
        //}
    }
}
