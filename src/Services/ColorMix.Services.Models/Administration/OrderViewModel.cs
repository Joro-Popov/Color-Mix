using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using ColorMix.Services.Models.Users;

namespace ColorMix.Services.Models.Administration
{
    public class OrderViewModel : IMapFrom<Order>,ICustomMappings
    {
        public Guid Id { get; set; }

        public string Receiver { get; set; }

        public string OrderDate { get; set; }

        public decimal OrderTotalPrice { get; set; }

        public string Status { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Order, OrderViewModel>()
                .ForMember(opt => opt.OrderDate,
                    opt => opt.MapFrom(x => x.OrderDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(opt => opt.Receiver,
                    opt => opt.MapFrom(x => $"{x.User.FirstName} {x.User.LastName}"));
        }
    }
}
