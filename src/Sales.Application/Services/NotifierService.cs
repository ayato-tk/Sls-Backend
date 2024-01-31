using Sales.Application.Interfaces;
using Sales.Core.DTOs;
using Sales.Domain.Common;
using Sales.Domain.Enums;
using Sales.Infra.Data.Interfaces;
using Sales.RabbitMQ.Client.Common.Enums;
using Sales.RabbitMQ.Client.Consumer.Interfaces;

namespace Sales.Application.Services;

public class NotifierService(
    IFileRepository fileRepository,
    IProjectRepository projectRepository,
    ICountsProducer countsProducer,
    IStatsProducer statsProducer
) : INotifierService
{
    public async Task<Result<NotifierDTO>> UpdateExtractionStatus(NotifierDTO notifierDTO)
    {
        if (notifierDTO.Status == ProjectStatus.Complete)
        {
            statsProducer.CreateStatsQueue("CreateStats", new StatsDTO
            {
                Email = notifierDTO.UserEmail,
                SessionId = notifierDTO.SessionId,
                UserId = notifierDTO.UserId ?? throw new Exception("User email is not define."),
                Token = notifierDTO.Token,
                ProjectType = ProjectType.Custom
            });

        }

        if (notifierDTO.Status == ProjectStatus.Error)
        {
            await fileRepository.UpdateProjectFileAsync(
                   "status",
                   FileStatus.Status[notifierDTO.Status],
                   notifierDTO.SessionId);
        }

        return notifierDTO;
    }

    public async Task<Result<NotifierDTO>> UpdateFileStatus(NotifierDTO notifierDTO)
    {
        if (notifierDTO.Status == ProjectStatus.Complete)
        {
            await projectRepository
                        .PostSendCustomCnpjAsync(
                            new InsertCnpjsDTO
                            {
                                Token = notifierDTO.Token,
                                SessionId = notifierDTO.SessionId,
                                RowsLimit = 100
                            }
                        );
                        
            countsProducer.CreateCountsQueue(CountsQueueType.QueueType[CountsType.Counts], notifierDTO);
        }

        if (notifierDTO.Status == ProjectStatus.Error)
        {
            await fileRepository.UpdateProjectFileAsync(
                   "status",
                   FileStatus.Status[notifierDTO.Status],
                   notifierDTO.SessionId);
        }

        return notifierDTO;
    }


}
