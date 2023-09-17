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
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;
        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<NotificationDto>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateNotification(CreateNotificationDto model)
        {
            try
            {
                var result = await _notificationService.CreateAsync(model);
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
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<NotificationDto>>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _notificationService.GetAllNotifications();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet("id")]
        [ProducesResponseType(typeof(SuccessResponse<NotificationDto>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetNotificationById([FromQuery] Guid id)
        {
            try
            {
                var result = await _notificationService.GetByIdAsync(id);
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
        [ProducesResponseType(typeof(SuccessResponse<bool>), (int)HttpStatusCode.NoContent)]

        public async Task<IActionResult> DeleteNotification([FromQuery] Guid id)
        {
            try
            {
                var result = await _notificationService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPut("id")]
        [Authorize]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> UpdateNotification([FromQuery] Guid id, UpdateNotificationDto model)
        {
            try
            {
                var result = await _notificationService.UpdateAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPut("mark-notifiction/id")]
        [Authorize]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> MarkNotification([FromQuery] Guid id, bool isRead)
        {
            try
            {
                var result = await _notificationService.MarkNotification(id, isRead);
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
