using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sales.RabbitMQ.Client.Common.Interfaces;

namespace Sales.RabbitMQ.Client.Common.Services;

public class ConsumerService : IConsumerService, IDisposable
{

    protected IModel Model { get; set; }

    // Definição de retentativas
    protected int MaxAttempt = 3;

    public readonly IConnection _connection;

    private string _queueName;

    protected AsyncEventingBasicConsumer Consumer { get; set; }

    public ConsumerService(IRabbitMqService rabbitMqService)
    {
        _queueName = "SalesDefaultQueue";
        _connection = rabbitMqService.CreateChannel();
        Model = _connection.CreateModel();
        Consumer = new AsyncEventingBasicConsumer(Model);
    }

    public void CreateQueue(string queueName, string exchange, string routeKey)
    {
        _queueName = queueName;
        Model.QueueDeclare(queueName, false, false, false);
        Model.ExchangeDeclare(exchange, ExchangeType.Direct, false, false);
        Model.QueueBind(queueName, exchange, routeKey);
    }

    public void Dispose()
    {
        try{
            if(Model.IsOpen) {
            
            foreach (var consumerTag in Consumer.ConsumerTags)
            {
                Model.BasicCancel(consumerTag);
            }
            Model.Close();
            Model.Dispose();
        }
        if(_connection.IsOpen)
        {
            _connection.Close();
            _connection.Dispose();
        }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Erro ao liberar recursos RabbitMQ: {ex.Message}");
        }
    }
}
