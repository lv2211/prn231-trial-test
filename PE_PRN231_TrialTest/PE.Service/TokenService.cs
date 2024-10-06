using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PE.Core.Commons;
using PE.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PE.Service
{
    public class TokenService(IOptions<JwtSettings> options)
    {
        private readonly JwtSettings _options = options.Value;

        public string GenerateToken(PremierLeagueAccount account)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.EmailAddress),
                new Claim(ClaimTypes.NameIdentifier, account.AccId.ToString()),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            };
            
            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_options.DurationInMinutes)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
