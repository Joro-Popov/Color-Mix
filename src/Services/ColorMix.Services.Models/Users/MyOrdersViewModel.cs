using System;
using System.Globalization;
using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Users
{
    public class MyOrdersViewModel : IMapFrom<Order>, ICustomMappings
    {
        public Guid Id { get; set; }
        
        public string OrderDate { get; set; }

        public decimal OrderTotalPrice { get; set; }

        public string Status { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Order, MyOrdersViewModel>()
                .ForMember(opt => opt.OrderDate,
                    opt => opt.MapFrom(x => x.OrderDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
