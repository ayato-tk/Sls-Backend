using System;
using System.Collections.Generic;
using System.Linq;
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

public class DashConsumer : ConsumerService, IDashConsumer
{

    private readonly IDashBoardRepository _dashboardRepository;

    private readonly IFileRepository _fileRepository;

    private const string QueueName = "DashQueue";
    private const string Exchange = "DashExchange";

    public DashConsumer(
        IDashBoardRepository dashBoardRepository,
        IFileRepository fileRepository,
        IRabbitMqService rabbitMqService
        ) : base(rabbitMqService)
    {
        _fileRepository = fileRepository;
        _dashboardRepository = dashBoardRepository;

        CreateQueue(QueueName, Exchange, "CreateDash");
    }

    public async Task GetDashQueue()
    {
        var requeue = 0;
        Consumer.Received += async (ch, ea) =>
        {
            var input = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
            var notifierDTO = JsonSerializerExtensions.Deserialize<NotifierDTO>(input);
            try
            {
                requeue++;
                var token = notifierDTO.Token ?? throw new Exception("User token not defined");
                List<DashConsumerDTO> dashHeaders =
                [
                    new DashConsumerDTO
                {
                    Table = "Stats",
                    Order = "Desc",
                },
                new DashConsumerDTO
                {
                    Table = "Cnae",
                    Order = "Desc",
                    RowsLimit= 5
                },
                new DashConsumerDTO
                {
                    Table = "CnaeDivision",
                    OrderBy = "Counter",
                    Order = "Desc",
                    RowsLimit = 5
                },
                new DashConsumerDTO
                {
                    Table = "CnaeSection",
                    OrderBy = "Counter",
                    Order = "Desc",
                    RowsLimit = 5
                },
                new DashConsumerDTO
                {
                    Table = "Invoicing",
                    OrderBy = "MinimumInvoicing",
                    Order = "Desc",
                    RowsLimit = 10
                },
                new DashConsumerDTO
                {
                    Table = "EmployeeQuantity",
                    OrderBy = "MinimumEmployeeQuantity",
                    Order = "Desc",
                    RowsLimit = 5
                },
                new DashConsumerDTO
                {
                    Table = "CompanySize",
                    OrderBy = "Counter",
                    Order = "Desc",
                    RowsLimit = 5
                },
                new DashConsumerDTO
                {
                    Table = "Region",
                    OrderBy = "Counter",
                    Order = "Desc",
                    RowsLimit = 2
                },
                new DashConsumerDTO
                {
                    Table = "RegionAbreviation",
                    OrderBy = "Counter",
                    Order = "Desc",
                    RowsLimit = 2
                },
                new DashConsumerDTO
                {
                    Table = "OpeningYears",
                    OrderBy = "Counter",
                    Order = "Desc",
                    RowsLimit = 5
                },
            ];

                IReadOnlyList<string> dashMinMax =
                [
                    "Invoicing", "EmployeeQuantity"
                ];

                IReadOnlyList<string> dashMinYears =
                [
                    "OpeningYears"
                ];

                var dashStats = dashHeaders.Select(async config =>
                {
                    var value = await _dashboardRepository.GetDashItemAsync(
                        new DashConsumerDTO
                        {
                            SessionId = notifierDTO.SessionId,
                            Table = config.Table,
                            Order = config.Order,
                            OrderBy = config.OrderBy,
                            RowsLimit = config.RowsLimit
                        },
                        token
                    );
                    return new
                    {
                        config.Table,
                        value
                    };
                });

                var minMaxStats = dashMinMax.Select(async table =>
                {
                    var value = await _dashboardRepository.GetDashMinMaxAsync(
                        new DashConsumerDTO
                        {
                            SessionId = notifierDTO.SessionId,
                            Table = table
                        },
                        notifierDTO.Token
                    );
                    return new
                    {
                        Table = $"{table}_MinMax",
                        value
                    };

                });

                var minYears = dashMinYears.Select(async table =>
                {
                    var value = await _dashboardRepository.GetDashYearsMinMaxAsync(
                      notifierDTO.SessionId,
                      notifierDTO.Token
                  );
                    return new
                    {
                        Table = $"{table}_MinMax",
                        value
                    };
                });

                var dashData = await Task.WhenAll(dashStats.Concat(minMaxStats).Concat(minYears));

                var resultDictionary = dashData.ToDictionary(
                    item => item.Table,
                    item => item.value
                );

                await _dashboardRepository.PutDashboardCountsAsync(
                notifierDTO.SessionId,
                JsonSerializerExtensions.Serialize(resultDictionary),
                notifierDTO.Token
                );

                await _fileRepository.UpdateProjectFileAsync(
                    "dashResponse",
                    JsonSerializerExtensions.Serialize(resultDictionary),
                    notifierDTO.SessionId
                );
                
                await _fileRepository.UpdateProjectFileAsync(
                    "status",
                    FileStatus.Status[ProjectStatus.Complete],
                    notifierDTO.SessionId);  

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
