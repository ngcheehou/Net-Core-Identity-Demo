using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Identity
{
    public class MyDepartment : IdentityRole
    {

        public WorkMode JobType { get; set; }
        //public virtual ICollection<MyEmployeeDepartment>? MyEmployeeDepartment { get; set; }

        //public virtual ICollection<MyDepartmentClaim>? MyDepartmentClaim { get; set; }

        //public MyDepartment() : base()
        //{

        //}

    }
}
