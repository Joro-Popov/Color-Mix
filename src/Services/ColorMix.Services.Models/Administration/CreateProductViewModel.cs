using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ColorMix.Services.Models.Administration
{
    public class CreateProductViewModel
    {
        private const string INVALID_NAME_LENGTH = "Въведете име с дължина между 3 и 32 символа !";
        private const string INVALID_SYMBOLS = "Полето съдържа невалидни символи !";
        private const string INVALID_IMAGE = "Изберете изображение за продукта !";
        private const string INVALID_CATEGORY = "Изберете категория от списъка !";
        private const string REQUIRED_FIELD = "Полето е задължително !";
        private const string INVALID_PRICE = "Въведете валидна стойност!";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [MinLength(3, ErrorMessage = INVALID_NAME_LENGTH)]
        [MaxLength(32, ErrorMessage = INVALID_NAME_LENGTH)]
        [RegularExpression("^[\u0410-\u044F\\s]+", ErrorMessage = INVALID_SYMBOLS)]
        public string Name { get; set; } 

        [Required(ErrorMessage = INVALID_IMAGE)]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Range(typeof(decimal),"0", "79228162514264337593543950335", ErrorMessage = INVALID_PRICE)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F\\s]+", ErrorMessage = INVALID_SYMBOLS)]
        public string Description { get; set; }

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
    }
}
