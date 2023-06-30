using DataLibrary.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static System.Net.WebRequestMethods;

namespace WebApp2.Pages.Identity
{
    public class VerifyWith2FAModel : PageModel
    {
        
        private readonly UserManager<MyEmployee> _userManager;

        public VerifyWith2FAModel( UserManager<MyEmployee> usermanager)
        {// _signInManager = signInManager;
            _userManager = usermanager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(User);



            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid. This user is not a 2FA employee.");
                return Page();

            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            bool isOTPValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, authenticatorCode);


            if (isOTPValid)
            {
                // Redirect user back to the originally intended page, which is stored in TempData
                returnUrl = TempData["ReturnUrl"] as string ?? Url.Content("~/");
                HttpContext.Session.SetString("Passed2FA", "true");  // To show the 2FA success
                return LocalRedirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Page();
            }
        }
    }
}
