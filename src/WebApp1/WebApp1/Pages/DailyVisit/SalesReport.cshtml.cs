using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp1.Constant;

namespace WebApp1.Pages.DailyVisit
{
    [Authorize(Policy = PagesNameConst.SalesReport)]
    public class SalesReportModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
