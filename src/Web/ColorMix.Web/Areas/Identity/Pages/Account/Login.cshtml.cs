using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ColorMix.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private const string REQUIRED_FIELD = "Полето е задължително !";
        private const string INVALID_NAME_LENGTH = "Въведете име с дължина между 3 и 32 символа !";
        private const string INVALID_PASSWORD_LENGTH = "Въведете парола с дължина между 6 и 100 символа !";
        private const string INVALID_USERNAME_OR_PASSWORD = "Невалидно потребителско име или парола !";

        private readonly SignInManager<ColorMixUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        
        public LoginModel(SignInManager<ColorMixUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        
        public class InputModel
        {
            [Required(ErrorMessage = REQUIRED_FIELD)]
            [MinLength(3, ErrorMessage = INVALID_NAME_LENGTH)]
            [MaxLength(32, ErrorMessage = INVALID_NAME_LENGTH)]
            public string Username { get; set; }

            [Required(ErrorMessage = REQUIRED_FIELD)]
            [StringLength(100, ErrorMessage = INVALID_PASSWORD_LENGTH, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }
        
        // TODO: Restrict authorized users
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null) 
        {
            returnUrl = returnUrl ?? "~/"; 
             
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, isPersistent:true, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl});
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, INVALID_USERNAME_OR_PASSWORD);
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
