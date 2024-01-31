using Sales.Core.DTOs;

namespace Sales.RabbitMQ.Client.Consumer.Interfaces;

public interface ICountsProducer
{
    void CreateCountsQueue(string message, NotifierDTO content);
}
