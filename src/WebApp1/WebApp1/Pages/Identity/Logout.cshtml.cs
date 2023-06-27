using DataLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp1.Identity
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly MyDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LogoutModel(
          SignInManager<IdentityUser> signInManager, MyDbContext context, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
        }
        public void OnGet()
        {
        }



        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);


            // Invalidate old sessions


            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
