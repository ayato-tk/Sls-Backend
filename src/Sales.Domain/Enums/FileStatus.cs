using System.Collections.Generic;

namespace Sales.Domain.Enums;

public static class FileStatus
{
    public static Dictionary<ProjectStatus, string> Status = new()
        {
            {
                ProjectStatus.Created, "CREATED"
            },
            {
                ProjectStatus.Processing, "PENDING"
            },
            {
                ProjectStatus.Complete, "DONE"
            },
            {
                ProjectStatus.Error, "ERROR_COUNTS"
            }
        };
}