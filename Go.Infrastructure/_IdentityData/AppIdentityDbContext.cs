using Go.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Go.Infrastructure._IdentityData
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser> //Custom Class Represent (IdentityUser,Defult(IdentityRole),Defult(key))
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);//to implement configrtion for DbSetFor(7) Table  in IdentityDbContext
        }
        
    }
}
