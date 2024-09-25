using Go.Core.Entities.Identity;
using Go.Core.Services.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Go.Application.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            // Privite Claims (PalyLoud(Clamis))
            var userAuth = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            // VERIFY SIGNATURE (Security Key)
            var keyAuth = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"]));

            // Register Claims (PalyLoud(Clamis))
            // Send Pram With Value To Create Obj Token
            var token = new JwtSecurityToken(

                // Register Claims (PalyLoud(Clamis))
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),

                //Privite Claims (PalyLoud(Clamis))
                claims: userAuth,

                //HEADER: ALGORITHM & TOKEN TYPE
                signingCredentials: new SigningCredentials(keyAuth,SecurityAlgorithms.HmacSha256Signature)
                );

            // Return Token Value
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
