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
    }
    
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, WorkingWhitsUsers>();
    }
    
    public static void AddMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingUser());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        services.AddSingleton(mapper);
    }
}