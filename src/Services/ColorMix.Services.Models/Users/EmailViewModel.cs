﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ColorMix.Services.Models.Users
{
    public class EmailViewModel
    {
        private const string INVALID_EMAIL_ADDRESS = "Невалиден E-mail адрес !";
        private const string REQUIRED_FIELD = "Полето е задължително !";
        private const string INVALID_SYMBOLS = "Полето съдържа невалидни символи !";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [EmailAddress(ErrorMessage = INVALID_EMAIL_ADDRESS)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F\\s]+", ErrorMessage = INVALID_SYMBOLS)]
        public string Title { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F\\s]+", ErrorMessage = INVALID_SYMBOLS)]
        public string Message { get; set; }
    }
}
