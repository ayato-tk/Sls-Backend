
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Sales.Core.Extensions;
using Sales.Domain.Entities;
using Sales.Infra.Data.Interfaces;

namespace Sales.Infra.Data.Repositories;

public class FileRepository : IFileRepository
{

    private readonly IConfiguration _configuration;

    private readonly IMongoCollection<ProjectFile> _projectCollection;

    private readonly IMapper _mapper;

    public FileRepository(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
        MongoClient mongoClient = new(_configuration["ConnectionStrings:MongoUrl"]);
        var database = mongoClient.GetDatabase("Sales");
        _projectCollection = database.GetCollection<ProjectFile>("Projects");
    }

    public async Task<ProjectFile> CreateProjectFileAsync(ProjectFile file)
    {
        try
        {
            await _projectCollection.InsertOneAsync(file);
            return file;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<List<ProjectFile>> GetAllProjectFilesAsync(string userId)
    {
        try
        {
            var filter = Builders<ProjectFile>.Filter.Eq("userId", ObjectId.Parse(userId));
            var project = await _projectCollection.FindAsync(filter);
            var projectsDb = await project.ToListAsync();
            foreach (var item in projectsDb)
            {
                item.CountsResponse = JsonSerializerExtensions.Deserialize<Counts>(item.CountsResponse);
                item.DashResponse = JsonSerializerExtensions.Deserialize<Dashboard>(item.DashResponse);
                item.PotentialCounts = JsonSerializerExtensions.Deserialize<Counts>(item.PotentialCounts);
            }

            return projectsDb;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<ProjectFile> GetProjectFileAsync(int id)
    {
        try
        {
            var filter = Builders<ProjectFile>.Filter.Eq("_id", id);
            var project = await _projectCollection.FindAsync(filter);
            return await project.FirstAsync();
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<ProjectFile> GetProjectFileBySessionIdAsync(int sessionId)
    {
        try
        {
            var filter = Builders<ProjectFile>.Filter.Eq("sessionId", sessionId);
            var project = await _projectCollection.FindAsync(filter);
            return await project.FirstAsync();
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task UpdateProjectFileAsync(string field, string value, int sessionId)
    {
        try
        {
            var filter = Builders<ProjectFile>.Filter.Eq("sessionId", sessionId);
            var update = Builders<ProjectFile>.Update.Set(field, BsonValue.Create(value));
            await _projectCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }
}
