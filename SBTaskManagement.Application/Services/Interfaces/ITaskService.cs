using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<SuccessResponse<TaskDTO>> GetByIdAsync(Guid id);
        Task<SuccessResponse<IEnumerable<TaskDTO>>> GetAllTask();
        Task<SuccessResponse<TaskDTO>> CreateAsync(CreateTaskDto model);
        Task<SuccessResponse<TaskDTO>> UpdateAsync(Guid id, UpdateTaskDto model);
        Task<SuccessResponse<bool>> DeleteAsync(Guid id);
        Task<SuccessResponse<IEnumerable<TaskDTO>>> GetTaskByStatusOrPriority(string status, string priority);
        Task<SuccessResponse<bool>> AssignTaskToUserAsync(Guid taskId, Guid userId);
        Task<SuccessResponse<bool>> AssignOrRemoveTaskFromProject(Guid taskId, Guid projectId);
        Task<SuccessResponse<IEnumerable<TaskDTO>>> GetTaskDueForCurrentWeek();
    }
}
