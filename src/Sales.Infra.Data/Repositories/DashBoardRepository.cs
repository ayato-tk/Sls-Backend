using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Sales.Core.DTOs;
using Sales.Core.Extensions;
using Sales.Domain.Entities;
using Sales.Infra.Data.Interfaces;

namespace Sales.Infra.Data.Repositories;

public class DashBoardRepository: IDashBoardRepository
{

    private readonly HttpClient _httpClient;

    private readonly IConfiguration _configuration;

    private string API_URL;

    public DashBoardRepository(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        API_URL = _configuration["ExternalUrls:SalesCore"];
    }

    public async Task<object> GetDashItemAsync(DashConsumerDTO dashDTO, string token)
    {
           try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var param = $"sessionId={dashDTO.SessionId}"+
                        $"&table={dashDTO.Table}"+
                        $"&rowsLimit={dashDTO.RowsLimit}"+
                        $"&orderBy={dashDTO.OrderBy}"+
                        $"&order={dashDTO.Order}";

            var request = await _httpClient.GetAsync($"{API_URL}/dashboard/stats?{param}");
            var response = await request.Content.ReadAsStringAsync();

            return JsonSerializerExtensions.Deserialize<object>(response) ?? throw new Exception("");;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<object> GetDashMinMaxAsync(DashConsumerDTO dashDTO, string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var param = $"sessionId={dashDTO.SessionId}&table={dashDTO.Table}";
            var request = await _httpClient.GetAsync($"{API_URL}/dashboard/minmax?{param}");
            var response = await request.Content.ReadAsStringAsync();

            return JsonSerializerExtensions.Deserialize<object>(response) ?? throw new Exception("");;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<object> GetDashYearsMinMaxAsync(int sessionId,  string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"{API_URL}/dashboard/years/minmax?sessionId={sessionId}&table=OpeningYears");
            var response = await request.Content.ReadAsStringAsync();

            return JsonSerializerExtensions.Deserialize<object>(response) ?? throw new Exception("");;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }

    public async Task<Dashboard> PutDashboardCountsAsync(int sessionId, string sessionCountJson, string token)
    {
        try
        {
            var content = new StringContent(
                JsonSerializerExtensions.Serialize(
                    new
                    {
                        sessionId,
                        sessionCountJson
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PutAsync($"{API_URL}/session/count?sessionId={sessionId}", content);
            var response = await request.Content.ReadAsStringAsync();

            var count = JsonSerializerExtensions.Deserialize<Dashboard>(response) ?? throw new Exception("");
            return count;
        }
        catch (Exception error)
        {
            throw new Exception(error.Message);
        }
    }
}
