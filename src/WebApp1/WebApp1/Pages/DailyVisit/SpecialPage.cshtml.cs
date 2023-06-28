using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp1.Constant;

namespace WebApp1.Pages.DailyVisit
{
    [Authorize(Policy = PagesNameConst.Special)]
    public class SpecialPageModel : PageModel
    {
        public string DocumentContent { get; set; }

        [Authorize(Policy = SpecialActionConst.CanViewDocument)]
        public void OnGet()
        {
            DocumentContent = "This is the document content.";
        }

        [Authorize(Policy = SpecialActionConst.CanEditDocument)]
        public void OnPost()
        {
            // Code to handle the form submission and edit the document
        }
    }
}
