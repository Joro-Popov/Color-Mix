using System.ComponentModel.DataAnnotations;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Users
{
    public class ProfileDataViewModel : IMapFrom<ColorMixUser>
    {
        private const string INVALID_NAME_LENGTH = "Въведете име с дължина между 3 и 32 символа !";
        private const string INVALID_SYMBOLS = "Полето съдържа невалидни символи !";
        private const string INVALID_AGE = "Въведете години между 16 и 80 !";
        private const string INVALID_EMAIL_ADDRESS = "Невалиден E-mail адрес !";
        private const string REQUIRED_FIELD = "Полето е задължително !";
        private const string INVALID_ZIP_CODE = "Невалиден пощенски код!";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [MinLength(3, ErrorMessage = INVALID_NAME_LENGTH)]
        [MaxLength(32, ErrorMessage = INVALID_NAME_LENGTH)]
        [RegularExpression("^[\u0410-\u044F]+", ErrorMessage = INVALID_SYMBOLS)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [MinLength(3, ErrorMessage = INVALID_NAME_LENGTH)]
        [MaxLength(32, ErrorMessage = INVALID_NAME_LENGTH)]
        [RegularExpression("^[\u0410-\u044F]+", ErrorMessage = INVALID_SYMBOLS)]
        public string LastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Range(16, 80, ErrorMessage = INVALID_AGE)]
        public int Age { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [EmailAddress(ErrorMessage = INVALID_EMAIL_ADDRESS)]
        public string Email { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F]+", ErrorMessage = INVALID_SYMBOLS)]
        public string AddressCountry { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression("^[\u0410-\u044F]+", ErrorMessage = INVALID_SYMBOLS)]
        public string AddressCity { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression(@"^[\u0410-\u044F\d.\s,]+", ErrorMessage = INVALID_SYMBOLS)]
        public string AddressStreet { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [RegularExpression(@"[0-9]{4}", ErrorMessage = INVALID_ZIP_CODE)]
        public int AddressZipCode { get; set; }
    }
}
