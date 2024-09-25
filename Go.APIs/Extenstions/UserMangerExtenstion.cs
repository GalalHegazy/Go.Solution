using Go.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Go.APIs.Extenstions
{
    public static class UserMangerExtenstion
    {
        public static async Task<ApplicationUser?> FindUserWithAddressAsync(this UserManager<ApplicationUser> _userManager
                                                                               ,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            // Eager Loading
            var user = await _userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.NormalizedEmail == email.ToUpper());

            return user;
        }
    }
}
