using System;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Categories;
using ColorMix.Services.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ColorMix.Services.DataServices.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void GetUserDataShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<ColorMixContext>()
                .UseInMemoryDatabase("ColorMix")
                .Options;
            
            var dbContext = new ColorMixContext(options);
            var id = Guid.NewGuid().ToString();

            dbContext.Users.Add(new ColorMixUser()
            {
                Id = id,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg"
            });

            dbContext.SaveChanges();

            var service = new UserService(dbContext);

            var user = service.GetUserData(id);

            Assert.NotNull(user);
        }

        [Fact]
        public void ChangePersonalDataShouldEditUserData()
        {
            var options = new DbContextOptionsBuilder<ColorMixContext>()
                .UseInMemoryDatabase("ColorMix")
                .Options;

            var dbContext = new ColorMixContext(options);
            var id = Guid.NewGuid().ToString();

            AutoMapperConfig.RegisterMappings(
                typeof(CategoryViewModel).Assembly
            );

            var user = new ColorMixUser()
            {
                Id = id,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg"
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            var service = new UserService(dbContext);
            
            var newUserData = new ProfileDataViewModel()
            {
                AddressStreet = "Street",
                AddressCity = "City",
                AddressCountry = "Country",
                FirstName = "Pesho",
                LastName = "Petrov",
                Age = 19,
                AddressZipCode = 1000,
                PhoneNumber = "1111111",
                Email = "email@mail.com"
            };

            service.ChangeUserData(user, newUserData);

            dbContext.Users.Update(user);

            dbContext.SaveChanges();

            var newUser = dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            Assert.NotSame(newUser, user);
        }
    }
}
