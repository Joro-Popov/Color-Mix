using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ColorMix.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ColorMix.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private const string INVALID_NAME_LENGTH = "Въведете име с дължина между 3 и 32 символа !";
        private const string INVALID_SYMBOLS = "Полето съдържа невалидни символи !";
        private const string INVALID_AGE = "Въведете години между 16 и 80 !";
        private const string INVALID_EMAIL_ADDRESS = "Невалиден E-mail адрес !";
        private const string INVALID_PASSWORD_LENGTH = "Въведете парола с дължина между 6 и 100 символа !";
        private const string INVALID_CONFIRM_PASSWORD = "Паролите не съвпадат !";
        private const string REQUIRED_FIELD = "Полето е задължително !";

        private readonly SignInManager<ColorMixUser> _signInManager;
        private readonly UserManager<ColorMixUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ColorMixUser> userManager,
            SignInManager<ColorMixUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = REQUIRED_FIELD)]
            [MinLength(3, ErrorMessage = INVALID_NAME_LENGTH)]
            [MaxLength(32, ErrorMessage = INVALID_NAME_LENGTH)]
            public string Username { get; set; }

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
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = REQUIRED_FIELD)]
            [StringLength(100, ErrorMessage = INVALID_PASSWORD_LENGTH, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = INVALID_CONFIRM_PASSWORD)]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new ColorMixUser
                {
                    UserName = Input.Username,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Age = Input.Age,
                    Email = Input.Email,
                    RegistrationDate = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (this._userManager.Users.Count() == 1)
                {
                    await this._userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await this._userManager.AddToRoleAsync(user, "User");
                }

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
