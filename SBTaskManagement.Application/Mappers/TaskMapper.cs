using AutoMapper;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Mappers
{
    public class TaskMapper : Profile
    {
        public TaskMapper()
        {
            CreateMap<CreateTaskDto, Domain.Entities.Task>().ReverseMap();
            CreateMap<Domain.Entities.Task, TaskDTO>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)).ReverseMap();
            CreateMap<Domain.Entities.Task, TaskDTO>()
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
