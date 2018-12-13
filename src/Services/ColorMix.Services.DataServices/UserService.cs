using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ColorMix.Services.DataServices
{
    public class UserService : IUserService
    {
        private readonly ColorMixContext dbContext;
        private readonly UserManager<ColorMixUser> userManager;

        public UserService(ColorMixContext dbContext, UserManager<ColorMixUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public ProfileDataViewModel GetUserData(ClaimsPrincipal claimsPrincipal)
        {
            var userId = userManager.GetUserId(claimsPrincipal);

            var userData = dbContext.Users.Where(u => u.Id == userId)
                .To<ProfileDataViewModel>().First();

            return userData;
        }

        public async Task ChangeUserData(ClaimsPrincipal claimsPrincipal, ProfileDataViewModel model)
        {
            var userId = userManager.GetUserId(claimsPrincipal);

            var user = await userManager.Users.Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == userId);

            user.Address.Country = model.AddressCountry;
            user.Address.City = model.AddressCity;
            user.Address.Street = model.AddressStreet;
            user.Address.ZipCode = model.AddressZipCode;
            user.PhoneNumber = model.PhoneNumber;

            await userManager.UpdateAsync(user);
        }
    }
}
