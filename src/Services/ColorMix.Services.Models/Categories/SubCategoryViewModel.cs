using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using System;
using SubCategory = ColorMix.Data.Models.SubCategory;

namespace ColorMix.Services.Models.Categories
{
    public class SubCategoryViewModel  :IMapFrom<SubCategory>, ICustomMappings
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int ProductsCount { get; set; }
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<SubCategory, SubCategoryViewModel>()
                .ForMember(opt => opt.ProductsCount,
                    opt => opt.MapFrom(x => x.Products.Count));
        }
    }
}
