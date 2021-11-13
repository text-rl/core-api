using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;

namespace CoreApi.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private const int DefaultMinutesDuration = 120;
        private readonly JwtSettings _settings;

        public TokenService(JwtSettings settings)
        {
            _settings = settings;
        }

        public Task<string> GenerateUserTokenAsync(Guid userId)
        {
            var credentials = GetCredentials();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            return GenerateToken(claims, credentials);
        }

        private Task<string> GenerateToken(Claim[] claims, SigningCredentials? credentials)
        {
            var token = new JwtSecurityToken(_settings.Issuer,
                _settings.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_settings.MinutesDuration ?? DefaultMinutesDuration),
                signingCredentials: credentials);
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private SigningCredentials? GetCredentials()
        {
            if (_settings.Key is null)
                throw new Exception("JWt Key is null please check your JwtSettings");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            return credentials;
        }
    }
}