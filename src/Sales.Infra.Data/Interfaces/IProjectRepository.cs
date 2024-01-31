using Sales.Core.DTOs;
using Sales.Domain.Entities;
namespace Sales.Infra.Data.Interfaces;

public interface IProjectRepository
{
    Task<Project> CreateCustomProjectAsync(ProjectDTO projectDto, string token);

    Task<Counts> GetProjectCountsAsync(StatsDTO statsDTO);

    Task<Counts> PostProjectCountsAsync(StatsDTO statsDTO);

    Task PutProjectCountsAsync(NotifierDTO notifierDTO, Counts counts);

    Task<bool> PostExtractCnpj(ExtractionDTO extractionDTO);

    Task<bool> PostSendCustomCnpjAsync(InsertCnpjsDTO insertCnpjsDTO);

    Task<Project> GetSalesProjectAsync(StatsDTO statsDTO);
}
