using AutoMapper;
using Models.DbModels;
using Services.Models.User;
using Services.Models.User.Requests;

namespace Mapper;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<User, UserModel>();
        CreateMap<UserModel, User>();
        
        CreateMap<AuthUserRequest, User>();
        
        CreateMap<UpdateEmailRequest, User>();
        
        CreateMap<UpdatePasswordRequest, User>();
    }
}