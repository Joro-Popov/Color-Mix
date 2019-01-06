using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly Mock<UserManager<ColorMixUser>> mockUserManager;
        private readonly ColorMixContext dbContext;

        public UserServiceTests()
        {
            this.mockUserManager = new Mock<UserManager<ColorMixUser>>(
                new Mock<IUserStore<ColorMixUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ColorMixUser>>().Object,
                new IUserValidator<ColorMixUser>[0],
                new IPasswordValidator<ColorMixUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ColorMixUser>>>().Object);

            this.dbContext = new ColorMixContext(new DbContextOptionsBuilder<ColorMixContext>()
                .UseInMemoryDatabase("ColorMix")
                .Options);
        }

        [Fact]
        public void GetUserDataShouldNotReturnNull()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(CategoryViewModel).Assembly
            );
            
            var roleId = Guid.NewGuid().ToString();

            var role = new IdentityRole()
            {
                Id = roleId,
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            this.dbContext.Roles.Add(role);
            this.dbContext.SaveChanges();

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

            this.dbContext.UserRoles.Add(new IdentityUserRole<string>() {UserId = userId, RoleId = roleId});
            this.dbContext.SaveChanges();

            var service = new UserService(dbContext,mockUserManager.Object);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Georgi"),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("name", "Georgi"),
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");

            var principal = new ClaimsPrincipal(identity);

            this.mockUserManager.Object.CreateAsync(user).GetAwaiter().GetResult();
            var userData = service.GetUserData(principal);

            Assert.NotNull(userData);
        }

        [Fact]
        public async Task ChangePersonalDataShouldEditUserData()
        {
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
            
            var service = new UserService(dbContext,mockUserManager.Object);
            
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

            var appUser = new ColorMixUser()
            {
                UserName = "Georgi"
            };

            await mockUserManager.Object.CreateAsync(appUser);

            //var principal = new TestPrincipal(new Claim("name", "Georgi"));

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Georgi"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("name", "Georgi"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            
            await service.ChangeUserData(claimsPrincipal, model);

            dbContext.Users.Update(user);

            dbContext.SaveChanges();

            var newUser = dbContext.Users.FirstOrDefault(x => x.Id == id);

            Assert.NotSame(newUser, user);
        }
    }
}
