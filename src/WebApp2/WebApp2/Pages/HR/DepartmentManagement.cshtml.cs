
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp2.Pages.HR
{
    public class DepartmentManagementModel : PageModel
    {
         
        private readonly RoleManager<MyDepartment> _roleManager;

        public DepartmentManagementModel(  RoleManager<MyDepartment> rolemanager)
        {
            
            _roleManager = rolemanager;
        }


        [BindProperty]
        public MyDepartment MyDepartment { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

           
            if (!ModelState.IsValid)
            {
                return Page();
            }


            if (await _roleManager.RoleExistsAsync(this.MyDepartment.NormalizedName))
            {
                ModelState.AddModelError("", "Name is exists.");

                return base.Page();
            }

            MyDepartment MyDepartment = new MyDepartment();


            MyDepartment.Name = this.MyDepartment.NormalizedName;
            MyDepartment.NormalizedName = this.MyDepartment.NormalizedName;

            await _roleManager.CreateAsync(MyDepartment);

            TempData["Success"] = "true";// ViewData to trigger the update successful modal.
            return RedirectToPage("./DepartmentManagement");
             
        }
    }
}
