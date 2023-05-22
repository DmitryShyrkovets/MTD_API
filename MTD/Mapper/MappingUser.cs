using AutoMapper;
using Models.DbModels;
using Services.ViewModels;

namespace Mapper;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<User, UserCli>();
        CreateMap<UserCli, User>();
    }
}