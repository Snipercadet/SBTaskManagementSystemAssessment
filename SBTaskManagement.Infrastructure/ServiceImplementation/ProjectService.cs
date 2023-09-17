using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Exceptions;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Entities;
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
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Domain.Entities.Task> _taskRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IMapper _mapper;
        public ProjectService(IRepository<Domain.Entities.Task> taskRepo, IMapper mapper, IProjectRepository projectRepo)
        {
            _mapper = mapper;
            _projectRepo = projectRepo;
            _taskRepo = taskRepo;
        }

        public async Task<SuccessResponse<ProjectDto>> CreateAsync(CreateProjectDto model)
        {
            if (model == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.PayloadCannotBeNull);
            }

            var project = _mapper.Map<Project>(model);
            await _projectRepo.AddAsync(project);
            await _projectRepo.SaveChangesAsync();

            var response = _mapper.Map<ProjectDto>(project);

            return new SuccessResponse<ProjectDto>
            {
                Data = response,
                Message = ResponseMessages.TaskCreationResponse
            };
        }


        public async Task<SuccessResponse<bool>> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.Invalid);

            var project = await _projectRepo.GetByIdAsync(id)
                ?? throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.UnableToRetrieveData);

            _projectRepo.Remove(project);
            await _projectRepo.SaveChangesAsync();

            return new SuccessResponse<bool>
            {
                Message = ResponseMessages.Successful
            };
        }
        public async Task<SuccessResponse<IEnumerable<ProjectDto>>> GetAllProjects()
        {

            var project = await _projectRepo.GetAll()
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var projectDto = _mapper.Map<IEnumerable<ProjectDto>>(project);
            return new SuccessResponse<IEnumerable<ProjectDto>>
            {
                Data = projectDto,
                Message = ResponseMessages.Successful
            };
        }
        public async Task<SuccessResponse<ProjectDto>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.Invalid);
            var project = await _projectRepo.GetProductWithListOfTasks(id)
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var projectDto = _mapper.Map<ProjectDto>(project);

            return new SuccessResponse<ProjectDto>
            {
                Data = projectDto,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<ProjectDto>> UpdateAsync(Guid id, UpdateProjectDto model)
        {
            if (id == Guid.Empty || model == null)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.PayloadCannotBeNull);

            var project = await _projectRepo.GetByIdAsync(id)
               ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            project.Name = model.Name;
            project.Description = model.Description;

            _projectRepo.Update(project);
            await _projectRepo.SaveChangesAsync();

            var response = _mapper.Map<ProjectDto>(project);
            return new SuccessResponse<ProjectDto>
            {
                Data = response,
                Message = ResponseMessages.Successful
            };
        }



    }
}
