using System;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ColorMix.Services.DataServices
{
    public class UserService : IUserService
    {
        private const string RECEIVER_EMAIL = "popov937@abv.bg";

        private readonly ColorMixContext dbContext;
        private readonly UserManager<ColorMixUser> userManager;

        public UserService(ColorMixContext dbContext, UserManager<ColorMixUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public ProfileDataViewModel GetUserData(ClaimsPrincipal principal)
        {
            var userId = this.userManager.GetUserId(principal);

            var userData = dbContext.Users.Where(u => u.Id == userId)
                .To<ProfileDataViewModel>().First();

            return userData;
        }

        public async Task ChangeUserData(ClaimsPrincipal principal, ProfileDataViewModel model)
        {
            var userId = userManager.GetUserId(principal);

            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Age = model.Age;
            user.Address.Country = model.AddressCountry;
            user.Address.City = model.AddressCity;
            user.Address.Street = model.AddressStreet;
            user.Address.ZipCode = model.AddressZipCode;
            user.PhoneNumber = model.PhoneNumber;

            await userManager.UpdateAsync(user);
        }
    }
}
