
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp1.Pages.HR
{
    public class DepartmentManagementModel : PageModel
    {
         
        private readonly RoleManager<IdentityRole> _roleManager;

        public DepartmentManagementModel(  RoleManager<IdentityRole> rolemanager)
        {
            
            _roleManager = rolemanager;
        }


        [BindProperty]
        public IdentityRole IdentityRole { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

           
            if (!ModelState.IsValid)
            {
                return Page();
            }


            if (await _roleManager.RoleExistsAsync(this.IdentityRole.NormalizedName))
            {
                ModelState.AddModelError("", "Name is exists.");

                return base.Page();
            }

            IdentityRole IdentityRole = new IdentityRole();


            IdentityRole.Name = this.IdentityRole.NormalizedName;
            IdentityRole.NormalizedName = this.IdentityRole.NormalizedName;

            await _roleManager.CreateAsync(IdentityRole);

            TempData["Success"] = "true";// ViewData to trigger the update successful modal.
            return RedirectToPage("./DepartmentManagement");
             
        }
    }
}
