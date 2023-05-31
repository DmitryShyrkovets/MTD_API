using AutoMapper;
using Models.DbModels;
using Services.DtoModels;

namespace Mapper;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}