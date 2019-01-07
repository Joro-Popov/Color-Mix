using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Services.Models.Administration;
using ColorMix.Services.Models.Users;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IMessageService
    {
        void SendMessage(EmailViewModel model);

        IEnumerable<MessageViewModel> GetAllUnAnsweredMessages();
    }
}
