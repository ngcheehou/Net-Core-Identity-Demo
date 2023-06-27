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

            options.AddPolicy(PagesNameConst.HR,
              policy => policy.RequireClaim(PagesNameConst.HR, "Yes"));
        }
    }
}
