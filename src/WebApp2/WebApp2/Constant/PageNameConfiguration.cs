using Microsoft.AspNetCore.Authorization;

namespace WebApp2.Constant
{
    public static class PageNameConfiguration
    {

        public static IServiceCollection ConfigurePageNameFunction(this IServiceCollection services)
        {
            PageNameDictionary.Instance.Add("Sales Report Page", PagesNameConst.SalesReport);
            PageNameDictionary.Instance.Add("Information Technology Page", PagesNameConst.IT);
            PageNameDictionary.Instance.Add("Manage Employee Page", PagesNameConst.ManageEmployee);
            PageNameDictionary.Instance.Add("Manage Department Access Page", PagesNameConst.ManageDepartment);
            PageNameDictionary.Instance.Add("Top Secret Page", PagesNameConst.TopSecret);
            PageNameDictionary.Instance.Add("HR Pages", PagesNameConst.HR);
            PageNameDictionary.Instance.Add("User Claim Demo Page", PagesNameConst.UserClaimDemo); 
            PageNameDictionary.Instance.Add("User Claim Action Demo Management", PagesNameConst.UserClaimActionDemoManagement);

            DemoUserClaimDictionary.Instance.Add("Can View Document", DemoUserClaimConst.CanViewDocument);
            DemoUserClaimDictionary.Instance.Add("Can Edit Document", DemoUserClaimConst.CanEditDocument);
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

            options.AddPolicy(PagesNameConst.UserClaimDemo,
              policy => policy.RequireClaim(PagesNameConst.UserClaimDemo, "Yes"));

            options.AddPolicy(PagesNameConst.UserClaimActionDemoManagement,
           policy => policy.RequireClaim(PagesNameConst.UserClaimActionDemoManagement, "Yes"));

            options.AddPolicy(DemoUserClaimConst.CanViewDocument,
              policy => policy.RequireClaim(DemoUserClaimConst.CanViewDocument, "Yes"));   
            
            options.AddPolicy(DemoUserClaimConst.CanEditDocument,
              policy => policy.RequireClaim(DemoUserClaimConst.CanEditDocument, "Yes"));
        }
    }
}
