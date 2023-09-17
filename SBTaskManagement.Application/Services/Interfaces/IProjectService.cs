using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task<SuccessResponse<IEnumerable<ProjectDto>>> GetAllProjects();
        Task<SuccessResponse<ProjectDto>> GetByIdAsync(Guid id);
        Task<SuccessResponse<ProjectDto>> CreateAsync(CreateProjectDto model);
        Task<SuccessResponse<ProjectDto>> UpdateAsync(Guid id, UpdateProjectDto model);
        Task<SuccessResponse<bool>> DeleteAsync(Guid id);
    }
}
