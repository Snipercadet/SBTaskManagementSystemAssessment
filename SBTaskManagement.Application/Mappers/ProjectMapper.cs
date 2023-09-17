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
    public class ProjectMapper : Profile
    {
        public ProjectMapper()
        {
            CreateMap<CreateProjectDto, Project>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project, ProjectDto>()
                .ForMember(dest=>dest.Tasks, opt=>opt.MapFrom(src=>src.TasksList));
        }
    }
}
