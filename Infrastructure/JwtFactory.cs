using BackEnd.Models.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Infrastructure
{
    public class JwtFactory
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;

        public JwtFactory(string issuer, string audience, string key)
        {
            _issuer = issuer;
            _audience = audience;
            _key = key;
        }

        public JwtSecurityToken GenerateToken(Claim[] claims) 
        {
            var encodedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

            return  new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(encodedKey, SecurityAlgorithms.HmacSha256));
        }

        public string GenerateTokenString(ApplicationUser user, string userRole) 
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRole)
            };

            var encodedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(encodedKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
