using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Administration
{
    public class MessageDetailsViewModel : IMapFrom<Message>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string EmailAddress { get; set; }

        public string Content { get; set; }

        public DateTime SendOn { get; set; }
    }
}
