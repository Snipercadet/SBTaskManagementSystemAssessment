using System;
using System.Collections.Generic;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Domain.Entities;

namespace SBTaskManagement.Application.Services.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        //TokenReturnHelper Authenticate(User user, IList<string> roles =null);
        //Guid GetUserIdFromAccessToken(string accessToken);
        //string GenerateRefreshToken(Guid userId);
        //bool ValidateRefreshToken(string refreshToken);
        (string, DateTime) CreateJwtToken(User user);
    }
}
