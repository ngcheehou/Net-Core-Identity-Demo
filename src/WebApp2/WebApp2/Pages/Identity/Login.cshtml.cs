using DataLibrary;
using DataLibrary.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApp2.Pages.Identity
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<MyEmployee> _signInManager;
        private readonly MyDbContext _context;
        private readonly UserManager<MyEmployee> _userManager;


        public LoginModel(
            SignInManager<MyEmployee> signInManager, MyDbContext context, UserManager<MyEmployee> userManager)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public class LoginInputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.Username);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, Input.Password, false, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {

                        return LocalRedirect(returnUrl);
                    }

                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2FA", new { ReturnUrl = returnUrl, RememberMe = false });
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return Page();
        }

    }
}
