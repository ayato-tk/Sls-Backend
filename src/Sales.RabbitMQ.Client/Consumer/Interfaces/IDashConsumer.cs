using System.Threading.Tasks;

namespace Sales.RabbitMQ.Client.Consumer.Interfaces;

public interface IDashConsumer
{
    Task GetDashQueue();
}
