using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Sales.Core.DTOs;
using Sales.Core.Extensions;
using Sales.Domain.Enums;
using Sales.Infra.Data.Interfaces;
using Sales.RabbitMQ.Client.Common.Interfaces;
using Sales.RabbitMQ.Client.Common.Services;
using Sales.RabbitMQ.Client.Consumer.Interfaces;

namespace Sales.RabbitMQ.Client.Consumer;

public class StatsConsumer : ConsumerService, IStatsConsumer
{
    private readonly IProjectRepository _projectRepository;

    private readonly IDashBoardRepository _dashboardRepository;

    private readonly ICountsProducer _countsProducer;

    private readonly IFileRepository _fileRepository;

    private const string QueueName = "StatsQueue";
    private const string Exchange = "StatsExchange";

    public StatsConsumer(
        IProjectRepository projectRepository,
        IFileRepository fileRepository,
        IDashBoardRepository dashBoardRepository,
        IRabbitMqService rabbitMqService,
        ICountsProducer countsProducer
        ) : base(rabbitMqService)
    {
        _projectRepository = projectRepository;
        _fileRepository = fileRepository;
        _dashboardRepository = dashBoardRepository;
        _countsProducer = countsProducer;

        CreateQueue(QueueName, Exchange, "CreateStats");
    }

    public async Task GetStatsQueue()
    {
        var requeue = 0;
        Consumer.Received += async (ch, ea) =>
        {
            var input = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
            var statsDTO = JsonSerializerExtensions.Deserialize<StatsDTO>(input);
            try
            {
                requeue++;

                await _projectRepository.PostProjectCountsAsync(statsDTO);
                Model.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception error)
            {
                await _fileRepository.UpdateProjectFileAsync(
                    "status",
                   FileStatus.Status[ProjectStatus.Error],
                    statsDTO.SessionId
                );
                await _fileRepository.UpdateProjectFileAsync(
                    "errorDetail",
                    error.Message,
                    statsDTO.SessionId
                );
                var ableToRequeue = requeue < MaxAttempt;
                Model.BasicNack(ea.DeliveryTag, false, ableToRequeue);
            }

        };

        Model.BasicConsume(QueueName, false, Consumer);
        await Task.CompletedTask;

    }

}
