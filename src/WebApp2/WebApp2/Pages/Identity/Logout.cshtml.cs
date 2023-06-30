using DataLibrary;
using DataLibrary.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp2.Identity
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<MyEmployee> _signInManager;

        private readonly MyDbContext _context;
        private readonly UserManager<MyEmployee> _userManager;

        public LogoutModel(
          SignInManager<MyEmployee> signInManager, MyDbContext context, UserManager<MyEmployee> userManager)
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
 
                return RedirectToPage();
             
        }
    }
}
