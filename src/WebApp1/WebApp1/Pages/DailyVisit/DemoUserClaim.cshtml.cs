using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp1.Constant;

namespace WebApp1.Pages.DailyVisit
{
    [Authorize(Policy = PagesNameConst.UserClaimDemo)]
    public class DemoUserClaimModel : PageModel
    {
        public string DocumentContent { get; set; }


        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public bool CanViewDocument { get; set; }
        public bool CanEditDocument { get; set; }



        public DemoUserClaimModel(UserManager<IdentityUser> usermanager, IAuthorizationService authorizationService)
        {
            _userManager = usermanager;
            _authorizationService = authorizationService;
        }
 

        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var userClaims = await _userManager.GetClaimsAsync(user);

            CanViewDocument = userClaims.Any(c => c.Type == DemoUserClaimConst.CanViewDocument && c.Value == "Yes");
            CanEditDocument = userClaims.Any(c => c.Type == DemoUserClaimConst.CanEditDocument && c.Value == "Yes");
        }


        public async Task OnPost()
        {
            var user = await _userManager.GetUserAsync(User);

            // Perform the authorization check for the CanEditDocument policy
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, DemoUserClaimConst.CanEditDocument);
            if (!authorizationResult.Succeeded)
            {
                // Handle the case where the user is not authorized to edit the document
                return;
            }

            var UserClaims = await _userManager.GetClaimsAsync(user);
            // Code to handle the form submission and edit the document
        }
    }
}
