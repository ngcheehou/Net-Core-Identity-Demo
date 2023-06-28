using Microsoft.AspNetCore.Authorization;

namespace WebApp1.Constant
{
    public static class PageNameConfiguration
    {

        public static IServiceCollection ConfigurPageNameFunction(this IServiceCollection services)
        {
            PageNameDictionary.Instance.Add("Sales Report Page", PagesNameConst.SalesReport);
            PageNameDictionary.Instance.Add("Information Technology Page", PagesNameConst.IT);
            PageNameDictionary.Instance.Add("Manage Employee Page", PagesNameConst.ManageEmployee);
            PageNameDictionary.Instance.Add("Manage Department Access Page", PagesNameConst.ManageDepartment);
            PageNameDictionary.Instance.Add("Top Secret Page", PagesNameConst.TopSecret);
            PageNameDictionary.Instance.Add("HR Pages", PagesNameConst.HR);
            PageNameDictionary.Instance.Add("Special Page", PagesNameConst.Special); 
            PageNameDictionary.Instance.Add("Special Action Management Page", PagesNameConst.SpecialActionManagement);

            SpecialActionDictionary.Instance.Add("Can View Document", SpecialActionConst.CanViewDocument);
            SpecialActionDictionary.Instance.Add("Can Edit Document", SpecialActionConst.CanEditDocument);
            return services;
        }


        public static void AddPageAccessPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(PagesNameConst.SalesReport,
                policy => policy.RequireClaim(PagesNameConst.SalesReport, "Yes"));

            options.AddPolicy(PagesNameConst.IT,
              policy => policy.RequireClaim(PagesNameConst.IT, "Yes"));

            options.AddPolicy(PagesNameConst.ManageEmployee,
                policy => policy.RequireClaim(PagesNameConst.ManageEmployee, "Yes"));

            options.AddPolicy(PagesNameConst.ManageDepartment,
              policy => policy.RequireClaim(PagesNameConst.ManageDepartment, "Yes"));

            options.AddPolicy(PagesNameConst.TopSecret,
              policy => policy.RequireClaim(PagesNameConst.TopSecret, "Yes"));

            options.AddPolicy(PagesNameConst.SpecialActionManagement,
           policy => policy.RequireClaim(PagesNameConst.SpecialActionManagement, "Yes"));



            options.AddPolicy(PagesNameConst.Special,
              policy => policy.RequireClaim(PagesNameConst.Special, "Yes"));

            options.AddPolicy(SpecialActionConst.CanViewDocument,
              policy => policy.RequireClaim(SpecialActionConst.CanViewDocument, "Yes"));   
            
            options.AddPolicy(SpecialActionConst.CanEditDocument,
              policy => policy.RequireClaim(SpecialActionConst.CanEditDocument, "Yes"));
        }
    }
}
