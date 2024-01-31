using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Sales.Core.DTOs;
using Sales.Core.Extensions;
using Sales.Domain.Enums;
using Sales.Infra.Data.Interfaces;
using Sales.RabbitMQ.Client.Common.Enums;
using Sales.RabbitMQ.Client.Common.Interfaces;
using Sales.RabbitMQ.Client.Common.Services;
using Sales.RabbitMQ.Client.Consumer.Interfaces;

namespace Sales.RabbitMQ.Client.Consumer;

public class CountsConsumer : ConsumerService, ICountsConsumer
{

    private readonly IProjectRepository _projectRepository;

    private readonly IFileRepository _fileRepository;

    private readonly IDashProducer _dashProducer;

    private const string QueueName = "CountsQueue";
    private const string Exchange = "CountsExchange";

    public CountsConsumer(
        IProjectRepository projectRepository,
        IFileRepository fileRepository,
        IDashProducer dashProducer,
        IRabbitMqService rabbitMqService) : base(rabbitMqService)
    {
        _fileRepository = fileRepository;
        _projectRepository = projectRepository;
        _dashProducer = dashProducer;

        CreateQueue(QueueName, Exchange, "CreateCounts");
    }

    public async Task GetCountsQueue()
    {
        var requeue = 0;
        Consumer.Received += async (ch, ea) =>
        {
            var input = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
            var notifierDTO = JsonSerializerExtensions.Deserialize<NotifierDTO>(input);
            try
            {
                requeue++;
                var counts = await _projectRepository.GetProjectCountsAsync(new StatsDTO
                {
                    UserId = notifierDTO.UserId ?? throw new Exception("UserId can not be null."),
                    Token = notifierDTO.Token
                });
                await _projectRepository.PutProjectCountsAsync(notifierDTO, counts);
                await _fileRepository.UpdateProjectFileAsync(
                    "countsResponse",
                  JsonSerializerExtensions.Serialize(counts),
                    notifierDTO.SessionId
                );
                _dashProducer.CreateDashQueue("CreateDash", notifierDTO);
                Model.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception error)
            {
                await _fileRepository.UpdateProjectFileAsync(
                    "status",
                   FileStatus.Status[ProjectStatus.Error],
                    notifierDTO.SessionId
                );
                await _fileRepository.UpdateProjectFileAsync(
                    "errorDetail",
                    error.Message,
                    notifierDTO.SessionId
                );
                var ableToRequeue = requeue < MaxAttempt;
                Model.BasicNack(ea.DeliveryTag, false, ableToRequeue);
            }
        };

        Model.BasicConsume(QueueName, false, Consumer);
        await Task.CompletedTask;
    }

}
