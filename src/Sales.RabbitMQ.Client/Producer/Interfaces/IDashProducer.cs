using Sales.Core.DTOs;

namespace Sales.RabbitMQ.Client.Consumer.Interfaces;

public interface IDashProducer
{
    void CreateDashQueue(string message, NotifierDTO content);
}
