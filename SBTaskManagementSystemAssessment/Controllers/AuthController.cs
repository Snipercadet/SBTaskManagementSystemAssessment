using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Infrastructure.ServicesImplementation;
using System.Net;

namespace SBTaskManagementSystemAssessment.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<string>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateUser(CreateUserDTO model)
        {
            try
            {
                var result = await _userService.RegisterAccount(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(SuccessResponse<UserLoginResponse>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> Login(UserLoginDto model)
        {
            try
            {
                var result = await _userService.Login(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<UserDto>>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet("id")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<UserDto>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetUserById([FromQuery] Guid id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }


        [HttpDelete("id")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<string>), (int)HttpStatusCode.NoContent)]

        public async Task<IActionResult> DeleteAccount([FromQuery] Guid userId)
        {
            try
            {
                var result = await _userService.DeleteAccount(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPut("update-password")]
        [Authorize]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> UpdatePassword([FromQuery] Guid id,UpdateUserPasswordDto model)
        {
            try
            {
                var result = await _userService.UpdatePassword(id, model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPut("id")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateUser([FromQuery] Guid id, UpdateUserDto model)
        {
            try
            {
                var result = await _userService.UpdateUser(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }


    }
}
