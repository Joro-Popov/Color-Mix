using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ColorMix.Data.Models;
using ColorMix.Services.Models.Users;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IUserService
    {
        ProfileDataViewModel GetUserData(ClaimsPrincipal principal);

        Task ChangeUserData(ClaimsPrincipal principal, ProfileDataViewModel model);

        void SendMessage(EmailViewModel model);
    }
}
