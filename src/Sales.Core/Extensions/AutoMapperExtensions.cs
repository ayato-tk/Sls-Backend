using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Sales.Core.DTOs;
using Sales.Core.ViewModels;
using Sales.Domain.Entities;
namespace Sales.Core.Extensions;

public static class AutoMapperExtensions
{
    public static void AddAutoMapperConfig(this IServiceCollection services)
    {
        var mapperConfiguration = new MapperConfiguration(config =>
        {
            config.CreateMap<Project, ProjectDTO>().ReverseMap();
            config.CreateMap<ProjectFile, CustomProjectViewModel>().ReverseMap();
            config.CreateMap<Project, ProjectFile>().ReverseMap();
        });

        var mapper = mapperConfiguration.CreateMapper();
        services.AddSingleton(mapper);
    }
}
