﻿using System;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;

namespace ColorMix.Services.Models.Products
{
    public class DetailsViewModel : IMapFrom<Product>, ICustomMappings
    {
        private const string REQUIRED_FIELD = "Полето е задължително !";

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Brand { get; set; }
        
        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Range(1, 10)]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        public ICollection<string> Sizes { get; set; }

        public decimal Total => Quantity * Price;
        
        public string Description { get; set; }
        
        public string Material { get; set; }
        
        public string Color { get; set; }
        
        public bool IsAvailable { get; set; }

        public ICollection<ProductViewModel> RandomProducts { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Product, DetailsViewModel>()
                .ForMember(opt => opt.Sizes,
                    opt => opt.MapFrom(x => x.Sizes.Select(s => s.Size.Abbreviation)));
        }
    }
}
