using AutoMapper;
using Mapper;
using Models.RepositoryInterfaces;
using Services.ServiceInterfaces;
using Repository;
using Services;

namespace MTD.DI;

public static class ServiceProviderExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<INoteService, NoteService>();
    }
    
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, WorkingWithUsers>();
        services.AddTransient<INoteRepository, WorkingWithNotes>();
    }
    
    public static void AddMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingUser());
            mc.AddProfile(new MappingNote());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        services.AddSingleton(mapper);
    }
}