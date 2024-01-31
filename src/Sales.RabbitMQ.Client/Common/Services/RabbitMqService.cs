using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Sales.RabbitMQ.Client.Common.Interfaces;

namespace Sales.RabbitMQ.Client.Common.Services;

public class RabbitMqService(IOptions<RabbitMqConfiguration> options) : IRabbitMqService
{
    private readonly RabbitMqConfiguration _configuration = options.Value;

    public IConnection CreateChannel()
    {
       ConnectionFactory connection = new()
       {
            UserName = _configuration.Username,
            Password = _configuration.Password,
            HostName = _configuration.HostName
       };
       connection.DispatchConsumersAsync = true;
       return connection.CreateConnection();
    }
}
