using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace ColorMix.Services.Models.Products
{
    public class DetailsViewModel : IMapFrom<Product>, ICustomMappings
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Material { get; set; }

        public string Color { get; set; }

        public bool IsAvailable { get; set; }

        public string Brand { get; set; }

        public ICollection<string> Sizes { get; set; }

        public ICollection<ProductViewModel> RandomProducts { get; set; }
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Product, DetailsViewModel>()
                .ForMember(opt => opt.Sizes,
                    opt => opt.MapFrom(x => x.Sizes.Select(s => s.Size.Abbreviation)));
        }
    }
}
