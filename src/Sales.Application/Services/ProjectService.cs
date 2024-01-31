using Sales.Application.Interfaces;
using Sales.Core.DTOs;
using Sales.Domain.Common;
using Sales.Infra.Data.Interfaces;
using Sales.Domain.Errors;
using Sales.RabbitMQ.Client.Consumer.Interfaces;
using AutoMapper;
using Sales.Domain.Entities;
using Sales.Domain.Enums;
using MongoDB.Bson;
using Sales.Core.Extensions;
namespace Sales.Application.Services;

public class ProjectService(
IProjectRepository projectRepository,
IStatsProducer statsProducer,
IMapper mapper,
IFileRepository fileRepository,
ITokenService tokenService
    ) : IProjectService
{

    private readonly IProjectRepository _projectRepository = projectRepository;

    private readonly IStatsProducer _statsProducer = statsProducer;

    private readonly IFileRepository _fileRepository = fileRepository;

    private readonly IMapper _mapper = mapper;

    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<ProjectFile>> CreateCustomProjectAsync(ProjectDTO projectDto)
    {
        try
        {
            var token = _tokenService.GetTokenForUser(projectDto.UserEmail, projectDto.UserId).RawData;

            var customProject = await _projectRepository.CreateCustomProjectAsync(projectDto, token);

            var sessionId = customProject.SessionId;

            await _projectRepository.PostExtractCnpj(
                 new ExtractionDTO
                 {
                     Token = token,
                     SessionId = sessionId,
                     RowsLimit = 100
                 }
             );

            var statsDTO = new StatsDTO
            {
                Email = customProject.UserEmail,
                SessionId = sessionId,
                UserId = projectDto.UserId,
                Token = token,
                ProjectType = ProjectType.Custom
            };

            var response = _mapper.Map<ProjectFile>(customProject);

            var fileName = customProject.Title ?? Guid.NewGuid().ToString();

            response.Status = FileStatus.Status[ProjectStatus.Created];
            response.CreatedAt = DateTime.UtcNow;
            response.Origin = FileOrigin.Origin[ProjectType.Custom];
            response.Count = 0;
            response.Customer = customProject.Customer;
            response.Filename = fileName;
            response.OriginalName = fileName;
            response.Title = fileName;
            response.UserId = projectDto.UserId;

            await _fileRepository.CreateProjectFileAsync(response);

            return response;
        }
        catch (Exception error)
        {
            return new BadRequestError("Bad request error", error.Message);
        }
    }

    public async Task<Result<List<ProjectFile>>> GetAllProjectsAsync(string userId)
    {
        try
        {
            return await _fileRepository.GetAllProjectFilesAsync(userId);
        }
        catch (Exception error)
        {
            return new BadRequestError("Bad request error", error.Message);
        }
    }

    public async Task<Result<Counts>> GetCountsAsync(StatsDTO statsDTO)
    {
        try
        {
            var counts = await _projectRepository.GetProjectCountsAsync(statsDTO);
            await _fileRepository.UpdateProjectFileAsync(
                    "countsResponse",
                    JsonSerializerExtensions.Serialize(counts),
                    statsDTO.SessionId
                );
            return counts;
        }
        catch (Exception error)
        {
            return new BadRequestError("Bad request error", error.Message);
        }
    }

    public async Task<ProjectFile> GetProjectFileBySessionIdAsync(int sessionId)
    {
        try
        {
            return await _fileRepository.GetProjectFileBySessionIdAsync(sessionId);
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }
}
