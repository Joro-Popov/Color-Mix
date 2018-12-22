using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Products
{
    public class CheckboxViewModel : ICustomMappings
    {
        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Category, CheckboxViewModel>()
                .ForMember(opt => opt.CategoryId,
                    opt => opt.MapFrom(x => x.Id));
        }
    }
}
