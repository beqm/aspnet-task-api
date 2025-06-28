using AutoMapper;
using Domain.Models;
using Application.Dtos;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserWithTasksDto>();
        CreateMap<Domain.Models.Task, TaskDto>();
    }
}
