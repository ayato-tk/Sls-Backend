using System.Text;
using Sales.Domain.Entities;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Sales.Core.DTOs;
using Sales.Infra.Data.Interfaces;
using Sales.Core.Extensions;

namespace Sales.Infra.Data.Repositories;

public class ProjectRepository : IProjectRepository
{

    private readonly HttpClient _httpClient;

    private readonly IConfiguration _configuration;

    private string API_URL;

    private string LOCAL_API;

    public ProjectRepository(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        API_URL = _configuration["ExternalUrls:SalesCore"];
        LOCAL_API = _configuration["LocalApi"];
    }

    public async Task<Project> CreateCustomProjectAsync(ProjectDTO projectDto, string token)
    {
        try
        {

            var content = new StringContent(
                    JsonSerializerExtensions.Serialize(projectDto),
                    Encoding.UTF8,
                    "application/json"
                );

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PostAsync($"{API_URL}/session/custom", content);


            var response = await request.Content.ReadAsStringAsync();

            if (!request.IsSuccessStatusCode) throw new Exception(response);

            var project = JsonSerializerExtensions.Deserialize<Project>(response) ?? throw new Exception(response);
            return project;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }

    }

    public async Task<Counts> GetProjectCountsAsync(StatsDTO statsDTO)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", statsDTO.Token);

            var request = await _httpClient.GetAsync($"{API_URL}/look/count?sessionId={statsDTO.SessionId}");
            var response = await request.Content.ReadAsStringAsync();

            var count = JsonSerializerExtensions.Deserialize<Counts>(response) ?? throw new Exception("");
            return count;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<Project> GetSalesProjectAsync(StatsDTO statsDTO)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", statsDTO.Token);

            var request = await _httpClient.GetAsync($"{API_URL}/session/{statsDTO.SessionId}");
            var response = await request.Content.ReadAsStringAsync();

            var project = JsonSerializerExtensions.Deserialize<Project>(response) ?? throw new Exception("");
            return project;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<bool> PostExtractCnpj(ExtractionDTO extractionDTO)
    {
        try{
            var sessionId = extractionDTO.SessionId;
            var webhookNotify = $"{LOCAL_API}/api/notify/extract";
            var content = new StringContent(
                JsonSerializerExtensions.Serialize(
                    new
                    {
                        SessionId = sessionId,
                        extractionDTO.RowsLimit,
                        WebhookNotify= webhookNotify
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", extractionDTO.Token
            );

            var request = await _httpClient.PostAsync($"{API_URL}/extract/cnpj", content);
            var response = await request.Content.ReadAsStringAsync();
            return true;
        }
        catch(Exception error){
            throw new Exception(error.Message);
        }
    }

    public async Task<Counts> PostProjectCountsAsync(StatsDTO statsDTO)
    {
        try
        {
            var sessionId = statsDTO.SessionId;
            var content = new StringContent(
                JsonSerializerExtensions.Serialize(
                    new
                    {
                        SessionId = sessionId,
                        WebhookNotify = $"{LOCAL_API}/api/notify/count/{sessionId}"
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", statsDTO.Token);

            var request = await _httpClient.PostAsync($"{API_URL}/look/v2/count?sessionId={sessionId}", content);
            var response = await request.Content.ReadAsStringAsync();

            var count = JsonSerializerExtensions.Deserialize<Counts>(response) ?? throw new Exception("");
            return count;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<bool> PostSendCustomCnpjAsync(InsertCnpjsDTO insertCnpjsDTO)
    {
        try
        {

            var content = new StringContent(
                JsonSerializerExtensions.Serialize(
                    new
                    {
                        insertCnpjsDTO.SessionId
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", 
            insertCnpjsDTO.Token);

            var extractCounts = await _httpClient.PostAsync($"{API_URL}/session/custom/cnpj", content);
            return true;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task PutProjectCountsAsync(NotifierDTO notifierDTO, Counts counts)
    {
         try
        {
            var content = new StringContent(
                JsonSerializerExtensions.Serialize(
                    new
                    {
                        notifierDTO.SessionId,
                        SessionCountJson = JsonSerializerExtensions.Serialize(counts)
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", notifierDTO.Token);

            var request = await _httpClient.PutAsync($"{API_URL}/session/count", content);
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }
}
