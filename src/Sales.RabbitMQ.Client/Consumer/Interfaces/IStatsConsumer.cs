

using System.Threading.Tasks;

namespace Sales.RabbitMQ.Client.Consumer.Interfaces;

public interface IStatsConsumer
{
    Task GetStatsQueue();
}
