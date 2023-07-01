

using DataLibrary.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp2.Pages.HR
{
    public class EmployeeManagementModel : PageModel
    {
         
        private readonly RoleManager<MyDepartment> _roleManager;
        private readonly UserManager<MyEmployee> _userManager;



        public EmployeeManagementModel( RoleManager<MyDepartment> rolemanager, UserManager<MyEmployee> usermanager)
        {
        
            _roleManager = rolemanager;
            _userManager = usermanager;
        }
         


        [BindProperty(SupportsGet = true)]
        public string? SelectedEmployeeID { get; set; }//user select

        [BindProperty(SupportsGet = true)]
        public string? SelectedDepartmentId { get; set; }//user select

        [BindProperty(SupportsGet = true)]
        public string? SelectedWorkTypeId { get; set; }//user select

        public List<MyDepartment> Departments { get; set; }

        public IList<SelectListItem>? DepartmentOptions { get; set; }
        public IList<SelectListItem>? EmployeeOptions { get; set; }
        public IList<SelectListItem>? WorkTypeOptions { get; set; }

        public async Task OnGet()
        {


            DepartmentOptions = _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id,
                    Selected = false
                }).ToList();

            EmployeeOptions = _userManager.Users
              .Select(r => new SelectListItem
              {
                  Text = r.UserName,
                  Value = r.Id
              }).ToList();

            WorkTypeOptions = Enum.GetValues(typeof(WorkMode))
                            .Cast<WorkMode>()
                                 .Select(w => new SelectListItem
                                 {
                                     Text = w.ToString(),
                                     Value = ((int)w).ToString()
                                 })
                            .ToList();


            if (!string.IsNullOrEmpty(SelectedEmployeeID))
            {
                var user = await _userManager.FindByIdAsync(SelectedEmployeeID);

                var roleName = (await _userManager.GetRolesAsync(user)).SingleOrDefault();

                if (roleName != null)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);

                    var defaultOption = DepartmentOptions.FirstOrDefault(o => o.Value == role.Id);
                    if (defaultOption != null)
                    {
                        defaultOption.Selected = true;

                    }
                     defaultOption = WorkTypeOptions.FirstOrDefault(o => o.Value == ((int)user.EmployeeWorkMode).ToString());
                    if (defaultOption != null)
                    {
                        defaultOption.Selected = true;
                    }
                }

            }


        }


        public async Task<IActionResult> OnPost()
        {

            var selectedDepartmentId = Request.Form["SelectedDepartmentId"].ToString();


            var user = await _userManager.FindByIdAsync(SelectedEmployeeID);
            var departmentName = (await _userManager.GetRolesAsync(user)).SingleOrDefault();


            if (departmentName != null)
            {
                await _userManager.RemoveFromRoleAsync(user, departmentName);
            }


            int workModeValue;

            if (int.TryParse(SelectedWorkTypeId, out workModeValue))
            {

                user.EmployeeWorkMode = (WorkMode)workModeValue;
            }

            await _userManager.UpdateAsync(user);

            if (!string.IsNullOrEmpty(selectedDepartmentId))
            {
                var role = await _roleManager.FindByIdAsync(selectedDepartmentId);
                string Groupname = role?.NormalizedName ?? "";


                await _userManager.AddToRoleAsync(user, Groupname);
            }

            TempData["Success"] = "true";// ViewData to trigger the update successful modal.
            return RedirectToPage("./EmployeeManagement");


        }

    }
}
