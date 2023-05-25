using AutoMapper;
using Models.DbModels;
using Services.ViewModels;

namespace Mapper;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<User, UserModel>();
        CreateMap<UserModel, User>();
    }
}