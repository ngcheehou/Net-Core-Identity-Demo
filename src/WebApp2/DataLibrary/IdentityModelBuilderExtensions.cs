using DataLibrary.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public static class IdentityModelBuilderExtensions
    {
        public static void CreateMyOwnIdentity(this ModelBuilder builder)
        {
            builder.Entity<MyEmployee>(b =>
            {
                b.ToTable("MyEmployee");

            });

            builder.Entity<MyDepartment>(b =>
            {
                b.ToTable("MyDepartment");


                b.HasMany(e => e.MyDepartmentClaim)
                    .WithOne(e => e.MyDepartment)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });



            builder.Entity<MyDepartmentClaim>(b =>
            {
                b.ToTable("MyDepartmentClaim");

            });

            builder.Entity<MyEmployeeDepartment>(b =>
            {
                b.ToTable("MyEmployeeDepartment");
            });
        }
    }
}
