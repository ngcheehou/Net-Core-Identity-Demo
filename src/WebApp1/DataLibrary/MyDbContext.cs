 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLibrary
{
    public class MyDbContext : IdentityDbContext<IdentityUser, IdentityRole, string,
      IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,
      IdentityRoleClaim<string>, IdentityUserToken<string>>
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

        }


    }
}
