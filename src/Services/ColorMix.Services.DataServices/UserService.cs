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
        private readonly ColorMixContext dbContext;

        public UserService(ColorMixContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ProfileDataViewModel GetUserData(string userId)
        {
            var userData = dbContext.Users.Where(u => u.Id == userId)
                .To<ProfileDataViewModel>().First();

            return userData;
        }

        public void ChangeUserData(string userId, ProfileDataViewModel model)
        {
            var user = this.dbContext.Users.FirstOrDefault(x => x.Id == userId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Age = model.Age;
            user.Address.Country = model.AddressCountry;
            user.Address.City = model.AddressCity;
            user.Address.Street = model.AddressStreet;
            user.Address.ZipCode = model.AddressZipCode;
            user.PhoneNumber = model.PhoneNumber;

            this.dbContext.Users.Update(user);
            this.dbContext.SaveChanges();
        }
    }
}
