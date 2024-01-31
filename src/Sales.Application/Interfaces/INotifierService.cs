using Sales.Core.DTOs;
using Sales.Domain.Common;

namespace Sales.Application.Interfaces;

public interface INotifierService
{
    Task<Result<NotifierDTO>> UpdateFileStatus(NotifierDTO notifierDTO);

    Task<Result<NotifierDTO>> UpdateExtractionStatus(NotifierDTO notifierDTO);
}
