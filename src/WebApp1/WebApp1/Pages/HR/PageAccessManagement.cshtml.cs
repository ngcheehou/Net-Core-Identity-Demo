using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp1.Constant;
using System.Security.Claims;
using System.Data;

namespace WebApp1.Pages.HR
{
    public class PageAccessManagementModel : PageModel
    {

        private readonly RoleManager<IdentityRole> _roleManager;
       




        [BindProperty]
        public IList<PageAccessModel> PageAccessName { get; set; } = new List<PageAccessModel>();
        public IList<Claim> RoleClaims { get; set; } = new List<Claim>();
        public class PageAccessModel
        {
            public string PageName { get; set; } = null!;
            public string ClaimType { get; set; } = null!;
            public bool Selected { get; set; }
        }




        [BindProperty(SupportsGet = true)]
        public string? DepartmentID { get; set; }//user select

        public PageAccessManagementModel(RoleManager<IdentityRole> rolemanager, UserManager<IdentityUser> usermanager)
        {

            _roleManager = rolemanager;
          

        }
        public IList<SelectListItem>? DepartmentOptions { get; set; }
        public async Task OnGet()
        {


            DepartmentOptions = _roleManager.Roles
            .Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();



            if (!string.IsNullOrEmpty(DepartmentID))
            {
                var role = await _roleManager.FindByIdAsync(DepartmentID);

                RoleClaims = await _roleManager.GetClaimsAsync(role);
            }


            foreach (var function in PageNameDictionary.Instance)
            {
                PageAccessName.Add(new PageAccessModel
                {
                    PageName = function.Key,
                    ClaimType = function.Value,

                    Selected = RoleClaims.Any(x => x.Type == function.Value)
                });
            }
            PageAccessName = PageAccessName.OrderBy(x => x.PageName).ToList();

        }


        public async Task<IActionResult> OnPostAsync()
        {

            try
            {

                if (!string.IsNullOrWhiteSpace(DepartmentID))
                {
                    var role = await _roleManager.FindByIdAsync(DepartmentID);
                    if (role != null)
                    {
                        var claims = await _roleManager.GetClaimsAsync(role);

                        var roleClaims = await _roleManager.GetClaimsAsync(role);

                        // Loop through each role claim and remove them
                        foreach (var roleClaim in roleClaims)
                        {
                            await _roleManager.RemoveClaimAsync(role, roleClaim);
                        }

                        foreach (var pagename in PageAccessName)
                        {


                            if (pagename.Selected)
                            {
                                await _roleManager.AddClaimAsync(role, new Claim(pagename.ClaimType, "Yes"));
                            }

                        }
                        TempData["Success"] = "true";// ViewData to trigger the update successful modal.
                        return RedirectToPage("./PageAccessManagement");
                    }
                    else
                    {
                        ModelState.AddModelError("", "department selected is invalid.");
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
