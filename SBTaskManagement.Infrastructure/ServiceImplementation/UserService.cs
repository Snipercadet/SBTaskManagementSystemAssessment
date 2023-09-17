using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Exceptions;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Entities;
using SBTaskManagement.Infrastructure.Repositories.Interfaces;

namespace SBTaskManagement.Infrastructure.ServicesImplementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration Configuration;
        private readonly AppConfig _config;
        private readonly IMapper _mapper;
        private readonly IJwtAuthenticationManager _jwtManager;


        public UserService(IUserRepository userRepository, IConfiguration configuration, IOptions<AppConfig> config, IMapper mapper, IJwtAuthenticationManager jwtManager)
        {
            _userRepository = userRepository;
            Configuration = configuration;
            _config = config.Value;
            _mapper = mapper;
            _jwtManager = jwtManager;
        }
       
        public async Task<SuccessResponse<string>> RegisterAccount(CreateUserDTO register)
        {
            
            if (string.IsNullOrEmpty(register.Username) || string.IsNullOrEmpty(register.Password))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.RequireUsernameandPassword);

            var confirmUsername = _userRepository.Exists(x => x.Username == register.Username);
            if (confirmUsername)
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.UsernameExist);

            var confirmEmail = _userRepository.Exists(x => x.Email == register.Email);
            if (confirmEmail)
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.EmailExist);

            var hashedpassword = HashingUtility.HashPassword(_config,register.Password);
            register.Password = hashedpassword.HashedValue;
            var newuser = _mapper.Map<User>(register);
            newuser.Salt = hashedpassword.Salt;
            await _userRepository.AddAsync(newuser);
            await _userRepository.SaveChangesAsync();

            return new SuccessResponse<string>()
            {
                Data = ResponseMessages.RegistrationSuccessful,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<UserLoginResponse>> Login(UserLoginDto loginModel)
        {
            
            var user = await _userRepository.FirstOrDefault(x => x.Username == loginModel.Username);

            if (user is null)
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.UsernameOrPasswordIncorrect);
            var hashPassword = HashingUtility.HashPassword(_config, loginModel.Password,user.Salt);
            if (!user.Password.SequenceEqual(hashPassword.HashedValue))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.PasswordIncorrect);

            var dashboardVm = new UserLoginResponse()
            {
                Username = user.Name,
                Email = user.Email,
                Id = user.Id
                
            };
            var (generateToken, expiryDate) = _jwtManager.CreateJwtToken(user);
            dashboardVm.Token = generateToken;
            dashboardVm.ExpiryDate = expiryDate;
            return new SuccessResponse<UserLoginResponse>
            {
                Data = dashboardVm,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<IEnumerable<UserDto>>> GetAllUsers()
        {

            var user = await _userRepository.GetUsersWithNavigationProp()
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var userDto = _mapper.Map<IEnumerable<UserDto>>(user);
            return new SuccessResponse<IEnumerable<UserDto>>
            {
                Data = userDto,
                Message = ResponseMessages.Successful
            };
        }
        public async Task<SuccessResponse<UserDto>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.Invalid);
            var user = await _userRepository.GetUserWithNavigationProp(id)
                ?? throw new RestException(System.Net.HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var userDto = _mapper.Map<UserDto>(user);

            return new SuccessResponse<UserDto>
            {
                Data = userDto,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<string>> DeleteAccount(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.Invalid);

            var user = await _userRepository.FirstOrDefault(x => x.Id == userId);
            if (user is null)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.ErrorWhileProcessing);

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();

            return new SuccessResponse<string>()
            {
                Data = ResponseMessages.Successful,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<string>> UpdatePassword(Guid id,UpdateUserPasswordDto updatePassword)
        {

            var user = await _userRepository.FirstOrDefault(x => x.Id == id)
                ?? throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.UserNotFound);

            var decriptedpassword = HashingUtility.HashPassword(_config,updatePassword.OldPassword,user.Salt);
            if (!user.Password.SequenceEqual(decriptedpassword.HashedValue))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.IncorrectOldPassword);

            if (updatePassword.NewPassword != updatePassword.ConfirmNewPassword)
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.PasswordDoNotMatch);

            var password = HashingUtility.HashPassword(_config, updatePassword.NewPassword);
            user.Password = password.HashedValue;
            user.Salt = password.Salt;
         
            await _userRepository.SaveChangesAsync();

            return new SuccessResponse<string>()
            {
                Data = ResponseMessages.PasswordUpdate,
                Message = ResponseMessages.Successful
            };

        }

        public async Task<SuccessResponse<bool>> UpdateUser(Guid id, UpdateUserDto parseruser)
        {

            var user = await _userRepository.FirstOrDefault(x => x.Id == id)
                ?? throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.ErrorWhileProcessing);

            user.Name = parseruser.Name;

      
            await _userRepository.SaveChangesAsync();

            var userDto = _mapper.Map<User>(parseruser);
            await _userRepository.AddAsync(user);


            return new SuccessResponse<bool>()
            {
                Data = true,
                Message = ResponseMessages.Successful
            };

        }
        
    }
}
