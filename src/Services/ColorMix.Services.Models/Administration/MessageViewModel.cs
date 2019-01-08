using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Administration
{
    public class MessageViewModel : IMapFrom<Message>,ICustomMappings
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string EmailAddress { get; set; }

        public string Content { get; set; }
        
        public string SendOn { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(opt => opt.SendOn,
                    opt => opt.MapFrom(x => x.SendOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(opt => opt.Content,
                    opt => opt.MapFrom(x => x.Content.Substring(0,25) + "..."));
        }
    }
}
