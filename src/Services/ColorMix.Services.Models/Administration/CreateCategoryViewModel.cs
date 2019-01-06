using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ColorMix.Services.Models.Categories
{
    public class CreateCategoryViewModel
    {
        private const string INVALID_NAME_LENGTH = "Въведете име с дължина между 3 и 32 символа !";
        private const string INVALID_SYMBOLS = "Полето съдържа невалидни символи !";
        private const string REQUIRED_FIELD = "Полето е задължително !";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F]+", ErrorMessage = INVALID_SYMBOLS)]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F\\s,]+", ErrorMessage = INVALID_SYMBOLS)]
        public string SubCаtegoryNames { get; set; }
    }
}
