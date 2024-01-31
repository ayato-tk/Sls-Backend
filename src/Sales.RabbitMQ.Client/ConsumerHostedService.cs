using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Sales.RabbitMQ.Client.Consumer.Interfaces;

namespace Sales.RabbitMQ;

public class ConsumerHostedService(
    IStatsConsumer statsConsumer, 
    IDashConsumer dashConsumer, 
    ICountsConsumer countsConsumer) : BackgroundService
{
    //REGISTER CONSUMERS
    private readonly IStatsConsumer _statsConsumer = statsConsumer;

    private readonly IDashConsumer _dashConsumer = dashConsumer;

    private readonly ICountsConsumer _countsConsumer = countsConsumer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _statsConsumer.GetStatsQueue();
        await _dashConsumer.GetDashQueue();
        await _countsConsumer.GetCountsQueue();
    }
}
