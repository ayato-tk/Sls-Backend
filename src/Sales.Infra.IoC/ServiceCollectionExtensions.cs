using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Interfaces;
using Sales.Application.JWT;
using Sales.Application.Services;
using Sales.Infra.Data.Interfaces;
using Sales.Infra.Data.Repositories;

namespace Sales.Infra.IoC;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddTransient<IDashBoardRepository, DashBoardRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddTransient<IFileRepository, FileRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddTransient<ITokenGenerator, TokenGenerator>();
        services.AddScoped<INotifierService, NotifierService>();

        return services;
    }

}