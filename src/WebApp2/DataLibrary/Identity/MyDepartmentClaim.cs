using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Identity
{
    public class MyDepartmentClaim : IdentityRoleClaim<string>
    {
        public virtual MyDepartment? MyDepartment { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

    }
}
