using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.RabbitMQ.Client.Common.Interfaces;
using Sales.RabbitMQ.Client.Common.Services;
using Sales.RabbitMQ.Client.Consumer;
using Sales.RabbitMQ.Client.Consumer.Interfaces;
using Sales.RabbitMQ.Client.Producer;

namespace Sales.RabbitMQ.Client;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRabbitClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(a => configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(a));
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
        services.AddSingleton<IConsumerService, ConsumerService>();
        services.AddSingleton<IStatsConsumer, StatsConsumer>();
        services.AddSingleton<IDashConsumer, DashConsumer>();
        services.AddScoped<IStatsProducer, StatsProducer>();
        services.AddTransient<IDashProducer, DashProducer>();
        services.AddTransient<ICountsConsumer, CountsConsumer>();
        services.AddTransient<ICountsProducer, CountsProducer>();
        services.AddHostedService<ConsumerHostedService>();
        
        return services;
    }
}
