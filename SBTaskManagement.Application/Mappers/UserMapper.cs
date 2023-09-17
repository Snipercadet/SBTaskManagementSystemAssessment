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
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<UpdateUserPasswordDto, User>();
            CreateMap<User, UserDto>().ReverseMap();
            
        }
    }
}
