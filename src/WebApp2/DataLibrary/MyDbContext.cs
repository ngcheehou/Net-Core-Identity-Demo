using DataLibrary.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class MyDbContext : IdentityDbContext<MyEmployee, MyDepartment, string,
      IdentityUserClaim<string>, MyEmployeeDepartment, IdentityUserLogin<string>,
      MyDepartmentClaim, IdentityUserToken<string>>
    {
        public MyDbContext() : base()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
         : base(options)
        {
        }

        public static MyDbContext Create()
        {
            return new MyDbContext();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.CreateMyOwnIdentity();

        }


    }
}
