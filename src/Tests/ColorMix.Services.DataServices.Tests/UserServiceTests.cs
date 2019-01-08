using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly ColorMixContext dbContext;
        private readonly UserService userService;

        public UserServiceTests()
        {
            this.dbContext = new ColorMixContext(new DbContextOptionsBuilder<ColorMixContext>()
                .UseInMemoryDatabase("ColorMix")
                .Options);

            this.userService = new UserService(dbContext);

            Mapper.Reset();

            AutoMapperConfig.RegisterMappings(
                typeof(CategoryViewModel).Assembly
            );
        }

        [Fact]
        public void GetUserDataShouldNotReturnNull()
        {
            var userId = Guid.NewGuid().ToString();

            var user = new ColorMixUser()
            {
                Id = userId,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg",
                UserName = "Georgi"
            };

            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();
            
            var userData = this.userService.GetUserData(userId);

            Assert.NotNull(userData);
        }

        [Fact]
        public void ChangePersonalDataShouldEditUserData()
        {
            var id = Guid.NewGuid().ToString();
            
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
            
            var model = new ProfileDataViewModel()
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
            
            this.userService.ChangeUserData(id,model);

            dbContext.Users.Update(user);
            dbContext.SaveChanges();

            var newUser = dbContext.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}
