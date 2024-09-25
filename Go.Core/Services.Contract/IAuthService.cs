using Go.Core.Entities.Identity;

namespace Go.Core.Services.Contract
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}
