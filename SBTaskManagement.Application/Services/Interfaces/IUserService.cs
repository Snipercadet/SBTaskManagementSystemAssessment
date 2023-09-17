using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<SuccessResponse<IEnumerable<UserDto>>> GetAllUsers();
        Task<SuccessResponse<UserDto>> GetByIdAsync(Guid id);
        Task<SuccessResponse<string>> RegisterAccount(CreateUserDTO register);
        Task<SuccessResponse<UserLoginResponse>> Login(UserLoginDto loginModel);
        Task<SuccessResponse<string>> DeleteAccount(Guid userId);
        Task<SuccessResponse<string>> UpdatePassword(Guid id, UpdateUserPasswordDto updatePassword);
        Task<SuccessResponse<bool>> UpdateUser(Guid id,UpdateUserDto parseruser);
    }
}
