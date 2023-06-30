using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp2.Constant;

namespace WebApp2.Pages.DailyVisit
{
    [Authorize(Policy = PagesNameConst.UserClaimDemo)]
    public class DemoUserClaimModel : PageModel
    {
        public string DocumentContent { get; set; }


        private readonly UserManager<MyEmployee> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public bool CanViewDocument { get; set; }
        public bool CanEditDocument { get; set; }



        public DemoUserClaimModel(UserManager<MyEmployee> usermanager, IAuthorizationService authorizationService)
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
            if (!CanEditDocument)
            {
                //sorry you are not allowed to Edit Documet
                return;
            }
            else
            {
                //proceed with edit document
            }
           
        }
    }
}
