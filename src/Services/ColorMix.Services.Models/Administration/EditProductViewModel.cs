using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using Microsoft.AspNetCore.Http;

namespace ColorMix.Services.Models.Products
{
    public class EditProductViewModel : IMapFrom<Product>, ICustomMappings
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile Image { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        public string Color { get; set; }

        public string Material { get; set; }

        public string Brand { get; set; }
        
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Sizes { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Product, EditProductViewModel>()
                .ForMember(opt => opt.Category,
                    opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(opt => opt.SubCategory,
                    opt => opt.MapFrom(x => x.SubCategory.Name))
                .ForMember(opt => opt.Sizes,
                    opt => opt.MapFrom(x => string.Join(",",x.Sizes.Select(s => s.Size.Abbreviation))));
        }
    }
}
