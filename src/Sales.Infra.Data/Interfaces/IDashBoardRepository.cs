using Sales.Core.DTOs;
using Sales.Domain.Entities;

namespace Sales.Infra.Data.Interfaces;

public interface IDashBoardRepository
{

   Task<object> GetDashItemAsync(DashConsumerDTO dashDTO, string token);

   Task<object> GetDashMinMaxAsync(DashConsumerDTO dashDTO, string token);

   Task<object> GetDashYearsMinMaxAsync(int sessionId, string token);

   Task<Dashboard> PutDashboardCountsAsync(int sessionId, string sessionCountJson, string token);
}
