using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private const string INVALID_NAME_LENGTH = "Въведете име с дължина между 3 и 32 символа !";
        private const string INVALID_SYMBOLS = "Полето съдържа невалидни символи !";
        private const string INVALID_CATEGORY = "Изберете категория от списъка !";
        private const string REQUIRED_FIELD = "Полето е задължително !";
        private const string INVALID_PRICE = "Въведете валидна стойност!";
        
        public Guid Id { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [MinLength(3, ErrorMessage = INVALID_NAME_LENGTH)]
        [MaxLength(32, ErrorMessage = INVALID_NAME_LENGTH)]
        [RegularExpression("^[\u0410-\u044F\\s]+", ErrorMessage = INVALID_SYMBOLS)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile Image { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = INVALID_PRICE)]
        public decimal Price { get; set; }


        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F\\s]+", ErrorMessage = INVALID_SYMBOLS)]
        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F\\s]+", ErrorMessage = INVALID_SYMBOLS)]
        public string Color { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F\\s]+", ErrorMessage = INVALID_SYMBOLS)]
        public string Material { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        public string Brand { get; set; }

        [Required(ErrorMessage = INVALID_CATEGORY)]
        public string Category { get; set; }

        [Required(ErrorMessage = INVALID_CATEGORY)]
        public string SubCategory { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
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
