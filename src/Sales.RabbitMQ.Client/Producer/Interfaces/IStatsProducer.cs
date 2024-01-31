using Sales.Core.DTOs;

namespace Sales.RabbitMQ.Client.Consumer.Interfaces;

public interface IStatsProducer
{
    void CreateStatsQueue(string message, StatsDTO content);
}
