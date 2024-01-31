using RabbitMQ.Client;

namespace Sales.RabbitMQ.Client.Common.Interfaces;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}
