using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Services
{
    public interface IAuthService
    {
        Task<string> GenerateToken(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<string> GenerateToken(string email, string password)
        {
            var user = await _userService.Get(email);

            if (user == null || !user.Password.Equals(password))
                return string.Empty;

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, email)
            };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(_configuration.GetValue<string>("Jwt:Secret")));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:ExpiresDay", 1)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
