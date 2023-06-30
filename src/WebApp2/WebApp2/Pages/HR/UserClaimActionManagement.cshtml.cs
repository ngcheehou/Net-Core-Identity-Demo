using DataLibrary.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp2.Constant;
using static WebApp2.Pages.HR.PageAccessManagementModel;

namespace WebApp2.Pages.HR
{
    public class UserClaimActionManagementModel : PageModel
    {
      

        public UserClaimActionManagementModel(UserManager<MyEmployee> usermanager)
        {
            _userManager = usermanager;
        }

        private readonly UserManager<MyEmployee> _userManager;

        [BindProperty]
        public IList<PersonalActionModel> PersonalActionName { get; set; } = new List<PersonalActionModel>();

        public IList<Claim> UserClaims { get; set; } = new List<Claim>();

        public class PersonalActionModel
        {
            public string ActionName { get; set; } = null!;
            public string ClaimType { get; set; } = null!;
            public bool Selected { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public string? EmployeeID { get; set; }//user select

        public IList<SelectListItem>? EmployeeOptions { get; set; }


     

        public async Task OnGetAsync()
        {
            EmployeeOptions = _userManager.Users
            .Select(r => new SelectListItem
            {
                Text = r.UserName,
                Value = r.Id
            }).ToList();


            if (!string.IsNullOrEmpty(EmployeeID))
            {
                var user = await _userManager.FindByIdAsync(EmployeeID);

                UserClaims = await _userManager.GetClaimsAsync(user);
            }


            foreach (var function in DemoUserClaimDictionary.Instance)
            {
                PersonalActionName.Add(new PersonalActionModel
                {
                    ActionName = function.Key,
                    ClaimType = function.Value,

                    Selected = UserClaims.Any(x => x.Type == function.Value)
                });
            }
            PersonalActionName = PersonalActionName.OrderBy(x => x.ActionName).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {

                if (!string.IsNullOrWhiteSpace(EmployeeID))
                {
                    var user = await _userManager.FindByIdAsync(EmployeeID);
                    if (user != null)
                    {
                        var claims = await _userManager.GetClaimsAsync(user);

                        var userClaims = await _userManager.GetClaimsAsync(user);

                        // Loop through each role claim and remove them
                        foreach (var userClaim in userClaims)
                        {
                            await _userManager.RemoveClaimAsync(user, userClaim);
                        }

                        foreach (var actionname in PersonalActionName)
                        {


                            if (actionname.Selected)
                            {
                                await _userManager.AddClaimAsync(user, new Claim(actionname.ClaimType, "Yes"));
                            }

                        }
                        TempData["Success"] = "true";// ViewData to trigger the update successful modal.
                        return RedirectToPage("./UserClaimActionManagement");
                    }
                    else
                    {
                        ModelState.AddModelError("", "employee selected is invalid.");
                    }
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
                return Page();

            }
            return Page();


        }

        public void OnGetResetSuccess()
        {
            ViewData["Success"] = "false";
        }
    }
}
