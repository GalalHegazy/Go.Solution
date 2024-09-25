using Go.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Go.Infrastructure._IdentityData.DataSeed
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any()){ 

                var user = new ApplicationUser()
                {
                    DisplayName = "Galal",
                    Email = "Galal@gmail.com",
                    PhoneNumber = "01111332278",
                    UserName="Galal.Hegazy"
                };
            
                await userManager.CreateAsync(user, "P@sswOrd1");   
            }
        }
    }
}
