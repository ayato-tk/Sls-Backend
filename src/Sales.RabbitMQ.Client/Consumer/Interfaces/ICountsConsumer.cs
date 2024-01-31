

using System.Threading.Tasks;

namespace Sales.RabbitMQ.Client.Consumer.Interfaces;

public interface ICountsConsumer
{
    Task GetCountsQueue();
}
