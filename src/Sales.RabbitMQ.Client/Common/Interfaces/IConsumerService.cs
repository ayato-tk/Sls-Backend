namespace Sales.RabbitMQ.Client.Common.Interfaces;

public interface IConsumerService
{
    void CreateQueue(string queueName, string exchange, string routeKe);

}
