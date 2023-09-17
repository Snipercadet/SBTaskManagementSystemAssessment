using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Exceptions;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Entities;
using SBTaskManagement.Domain.Enums;
using SBTaskManagement.Infrastructure.ExternalIntegrations;
using SBTaskManagement.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.ServicesImplementation
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IRepository<Project> _projectRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IEmailTemplateService _emailTemplateService;
        private ETaskStatus _taskStatus;
        private EPriority _taskPriority;
        public TaskService(ITaskRepository taskRepo, IMapper mapper, IRepository<Project> projectRepo, INotificationService notificationService, IEmailTemplateService emailTemplateService, IRepository<User> userRepo)
        {
            _taskRepo = taskRepo;
            _mapper = mapper;
            _projectRepo = projectRepo;
            _notificationService = notificationService;
            _emailTemplateService = emailTemplateService;
            _userRepo = userRepo;
        }

        public async Task<SuccessResponse<TaskDTO>> CreateAsync(CreateTaskDto model)
        {
            if (model == null)
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.PayloadCannotBeNull);
            }

            var task = _mapper.Map<Domain.Entities.Task>(model);
           
            await _taskRepo.AddAsync(task);
            await _taskRepo.SaveChangesAsync();

            var response = _mapper.Map<TaskDTO>(task);

            return new SuccessResponse<TaskDTO>
            {
                Data = response,
                Message = ResponseMessages.TaskCreationResponse
            };
        }

        public async Task<SuccessResponse<bool>> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.Invalid);

            var task = await _taskRepo.GetByIdAsync(id)
                ?? throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.UnableToRetrieveData);

            _taskRepo.Remove(task);
            await _taskRepo.SaveChangesAsync();

            return new SuccessResponse<bool>
            {
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<IEnumerable<TaskDTO>>> GetAllTask()
        {
            
            var task = await _taskRepo.GetAll()
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var taskDto = _mapper.Map<IEnumerable<TaskDTO>>(task);
            return new SuccessResponse<IEnumerable<TaskDTO>>
            {
                Data = taskDto,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<TaskDTO>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.Invalid);
            var task = await _taskRepo.GetTaskWithNavigationProp(id)
                ?? throw new RestException(System.Net.HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var taskDto = _mapper.Map<TaskDTO>(task);
            return new SuccessResponse<TaskDTO>
            {
                Data = taskDto,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<TaskDTO>> UpdateAsync(Guid id, UpdateTaskDto model)
        {
            if (id == Guid.Empty || model == null)
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.PayloadCannotBeNull);

            var task = await _taskRepo.GetByIdAsync(id)
               ?? throw new RestException(System.Net.HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            task.Title = model.Title;
            task.Description = model.Description;
            task.Priority = model.Priority;
            task.Status = model.Status;

            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();

            var response = _mapper.Map<TaskDTO>(task);
            return new SuccessResponse<TaskDTO>
            {
                Data = response,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<bool>> AssignTaskToUserAsync(Guid taskId, Guid userId)
        {
            var response = new SuccessResponse<bool>();
            var task = await _taskRepo.GetTaskWithNavigationProp(taskId)
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var user = await _userRepo.FirstOrDefault(x => x.Id == userId)
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            // Assign and remove the task to the user
            if (task.User != user)
            {
                task.User = user;
                response.Message = ResponseMessages.TaskAssignmentSuccessful;
            }
            else
            {
                task.User = null;
                response.Message = ResponseMessages.TaskRemovalSuccessful;
            }
            task.Status = ETaskStatus.InProgress;

            // Notify the user about the task assignment
            var message = _emailTemplateService.GetTaskAssignmentNotificationTemplate(task.User.Name, task.Title, task.Description, task.DueDate.ToString());
            var notification = new CreateNotificationDto
            {
                Message = message,
                Type = ENotificationType.NewTaskAssignment,
                UserId = userId,
                TaskId = taskId
            };

            await _notificationService.CreateAsync(notification);
            await _taskRepo.SaveChangesAsync();

            return new SuccessResponse<bool>
            {
                Data = true,
                Message = ResponseMessages.Successful
            };

        }
       
        public async Task<SuccessResponse<IEnumerable<TaskDTO>>> GetTaskByStatusOrPriority(string status, string priority)
        {
            if (!Enum.TryParse(status, true, out ETaskStatus taskStatus))
            {
                taskStatus = ETaskStatus.Default; // Default value for status when it's not provided or invalid.
            }

            if (!Enum.TryParse(priority, true, out EPriority taskPriority))
            {
                taskPriority = EPriority.Default; // Default value for priority when it's not provided or invalid.
            }

            var query = _taskRepo.Query(x => x.Id != Guid.Empty);

            if (taskStatus != ETaskStatus.Default)
            {
                query = query.Where(x => x.Status == taskStatus);
            }

            if (taskPriority != EPriority.Default)
            {
                query = query.Where(x => x.Priority == taskPriority);
            }

            var queryProjection = query.ProjectTo<TaskDTO>(_mapper.ConfigurationProvider);
            var response = await queryProjection.ToListAsync();

            return new SuccessResponse<IEnumerable<TaskDTO>>
            {
                Data = response,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<IEnumerable<TaskDTO>>> GetTaskDueForCurrentWeek()
        {
            // Calculate the start of the week (Sunday)
            var currentDate = DateTime.UtcNow.AddHours(1).ToUniversalTime();
            int diff = currentDate.DayOfWeek - DayOfWeek.Sunday;
            if (diff < 0)
                diff += 7; // Handle negative difference when the current date is Sunday
            var currentWeekStartDate = currentDate.Date.AddDays(-diff).ToUniversalTime();
            // Calculate the end of the week (Saturday)
            var currentWeekEndDate = currentWeekStartDate.AddDays(6).ToUniversalTime();

            var taskInCurrentWeek = await _taskRepo.Query(x => x.Id != Guid.Empty)
                .Where(x => x.CreatedAt >= currentWeekStartDate && x.CreatedAt <= currentWeekEndDate).ToListAsync()
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.ErrorWhileProcessing);

            var currentWeekTaskList = _mapper.Map<List<TaskDTO>>(taskInCurrentWeek);
            return new SuccessResponse<IEnumerable<TaskDTO>>
            {
                Data = currentWeekTaskList,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<bool>> AssignOrRemoveTaskFromProject(Guid taskId, Guid projectId)
        {
            var response = new SuccessResponse<bool>();
            if (projectId == Guid.Empty || taskId == Guid.Empty)
                throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.Invalid);

            var project = await _projectRepo.FirstOrDefault(x => x.Id == projectId)
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);
            var task = await _taskRepo.FirstOrDefault(x => x.Id == taskId)
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            if (task.Project != project)
            {
                task.Project = project;
                response.Message = ResponseMessages.TaskAssignmentSuccessful;
            }
            else
            {
                task.Project = null;
                response.Message = ResponseMessages.TaskRemovalSuccessful;
            }
            await _taskRepo.SaveChangesAsync();
            return response;
        }
    }
}
