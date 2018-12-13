using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ColorMix.Services.Models.Users;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IUserService
    {
        ProfileDataViewModel GetUserData(ClaimsPrincipal claimsPrincipal);

        Task ChangeUserData(ClaimsPrincipal claimsPrincipal, ProfileDataViewModel model);
    }
}
