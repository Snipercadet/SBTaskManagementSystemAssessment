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
    [Route("api/project")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectController> _logger;
        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<ProjectDto>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateProject(CreateProjectDto model)
        {
            try
            {
                var result = await _projectService.CreateAsync(model);
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
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProjectDto>>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _projectService.GetAllProjects();
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
        [ProducesResponseType(typeof(SuccessResponse<ProjectDto>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetProjectId([FromQuery] Guid id)
        {
            try
            {
                var result = await _projectService.GetByIdAsync(id);
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

        public async Task<IActionResult> DeleteProject([FromQuery] Guid id)
        {
            try
            {
                var result = await _projectService.DeleteAsync(id);
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
        [ProducesResponseType(typeof(SuccessResponse<ProjectDto>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> UpdateProject([FromQuery] Guid id, UpdateProjectDto model)
        {
            try
            {
                var result = await _projectService.UpdateAsync(id, model);
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
