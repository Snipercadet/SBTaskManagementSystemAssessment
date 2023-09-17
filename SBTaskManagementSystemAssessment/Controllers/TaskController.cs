using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Infrastructure.ServicesImplementation;
using System.Globalization;
using System.Net;

namespace SBTaskManagementSystemAssessment.Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;
        public TaskController(ITaskService projectService, ILogger<TaskController> logger)
        {
            _taskService = projectService;
            _logger = logger;
        }


        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<TaskDTO>), (int)HttpStatusCode.Created)]

        public async Task<IActionResult> CreateTask(CreateTaskDto model)
        {
            try
            {
                var result = await _taskService.CreateAsync(model);
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
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<TaskDTO>>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _taskService.GetAllTask();
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
        [ProducesResponseType(typeof(SuccessResponse<TaskDTO>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetTaskById([FromQuery] Guid id)
        {
            try
            {
                var result = await _taskService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet("current-week-due-task")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<TaskDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTaskDueForTheWeek()
        {
            try
            {
                var result = await _taskService.GetTaskDueForCurrentWeek();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet("task-status-or-priority")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<TaskDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTaskByStatusOrPriority([FromQuery] string status, string priority)
        {
            try
            {
                var result = await _taskService.GetTaskByStatusOrPriority(status,priority);
                return Ok(result);
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

        public async Task<IActionResult> UpdateTask([FromQuery] Guid id, UpdateTaskDto model)
        {
            try
            {
                var result = await _taskService.UpdateAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPut("assign-task/userId/taskId")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssignTaskToUser([FromQuery] Guid userId, [FromQuery] Guid taskId)
        {
            try
            {
                var result = await _taskService.AssignTaskToUserAsync(taskId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPut("assign-task/projectId/taskId")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssignOrRemoveTaskFromProject([FromQuery] Guid projectId, [FromQuery] Guid taskId)
        {
            try
            {
                var result = await _taskService.AssignOrRemoveTaskFromProject(taskId, projectId);
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

        public async Task<IActionResult> DeleteTask([FromQuery] Guid id)
        {
            try
            {
                var result = await _taskService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

    }
}
