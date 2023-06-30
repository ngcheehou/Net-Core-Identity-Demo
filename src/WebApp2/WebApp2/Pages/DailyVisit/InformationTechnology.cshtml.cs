using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp2.Constant;

namespace WebApp2.Pages.DailyVisit
{
    [Authorize(Policy = PagesNameConst.IT)]
    public class InformationTechnologyModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
