using MongoDB.Bson;
using Sales.Domain.Entities;

namespace Sales.Infra.Data.Interfaces;

public interface IFileRepository
{
    Task<ProjectFile> CreateProjectFileAsync(ProjectFile file);

    Task<ProjectFile> GetProjectFileAsync(int id);

    Task<ProjectFile> GetProjectFileBySessionIdAsync(int sessionId);

    Task<List<ProjectFile>> GetAllProjectFilesAsync(string userId);

    Task UpdateProjectFileAsync(string field, string value, int sessionId);
}
