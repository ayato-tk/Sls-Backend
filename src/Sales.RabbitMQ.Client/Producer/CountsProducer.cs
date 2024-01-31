using System.Text;
using RabbitMQ.Client;
using Sales.Core.DTOs;
using Sales.Core.Extensions;
using Sales.RabbitMQ.Client.Common.Interfaces;
using Sales.RabbitMQ.Client.Consumer.Interfaces;

namespace Sales.RabbitMQ.Client.Producer;

public class CountsProducer(IRabbitMqService RabbitMqService) : ICountsProducer
{
    private readonly IModel? _model = RabbitMqService.CreateChannel().CreateModel();

    public void CreateCountsQueue(string message, NotifierDTO content)
    {
         var body = Encoding.UTF8.GetBytes(JsonSerializerExtensions.Serialize(content));
        _model.BasicPublish("CountsExchange", message, null, body);
    }

}
