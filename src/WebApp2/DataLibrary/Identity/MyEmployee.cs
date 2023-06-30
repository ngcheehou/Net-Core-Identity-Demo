using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Identity
{
    public class MyEmployee : MyEmployee

    {
        public WorkMode EmployeeWorkMode { get; set; }

        public virtual ICollection<MyEmployeeDepartment>? MyEmployeeDepartment { get; set; }

        public MyEmployee()
          : base()
        {

        }

        public MyEmployee(string userName)
            : base(userName)
        {

        }
    }
}
