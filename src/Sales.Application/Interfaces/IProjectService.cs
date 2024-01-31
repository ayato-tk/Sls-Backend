using Sales.Core.DTOs;
using Sales.Domain.Common;
using Sales.Domain.Entities;

namespace Sales.Application.Interfaces;

public interface IProjectService
{
    Task<Result<ProjectFile>> CreateCustomProjectAsync(ProjectDTO projectDto);

    Task<Result<List<ProjectFile>>> GetAllProjectsAsync(string userId);

    Task<Result<Counts>> GetCountsAsync(StatsDTO statsDTO);

    Task<ProjectFile> GetProjectFileBySessionIdAsync(int sessionId);
    
}