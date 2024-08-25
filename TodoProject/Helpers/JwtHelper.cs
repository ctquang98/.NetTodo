using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoProject.models;

namespace TodoProject.Helpers
{
    public class JwtHelper
    {
        private readonly string secretKey;
        private readonly string issuer;
        private readonly string audience;

        public JwtHelper(string secretKey, string issuer, string audience)
        {
            this.secretKey = secretKey;
            this.issuer = issuer;
            this.audience = audience;
        }

        public string generateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
            };

            foreach (var role in user.Roles)
            {
                if (!string.IsNullOrWhiteSpace(role?.Name))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
