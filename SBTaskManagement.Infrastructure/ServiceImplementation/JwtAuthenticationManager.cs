using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SBTaskManagement.Application.Exceptions;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace SBTaskManagement.Infrastructure.ServicesImplementation
{
    public class JwtAuthenticationManager: IJwtAuthenticationManager
    {
        private readonly JwtConfigSettings _jwtConfigSettings;

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly SecurityKey _issuerSigningKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfiguration _config;

        public JwtAuthenticationManager(IOptions<JwtConfigSettings> jwtConfigSettings, IConfiguration config)
        {
            _jwtConfigSettings = jwtConfigSettings.Value;
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfigSettings.Secret));
            _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfigSettings.Secret))
            };
            _config = config;
        }

        public (string, DateTime) CreateJwtToken(User user)
        {
            var roleClaims = new List<Claim>();
           
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", user.Username)
            }
                .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigSettings.Secret));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtConfigSettings.ValidIssuer,
                audience: _jwtConfigSettings.Audience,
            claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtConfigSettings.TokenLifespan)),
                signingCredentials: signingCredentials);
            return (new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo);
        }
        
    }
}