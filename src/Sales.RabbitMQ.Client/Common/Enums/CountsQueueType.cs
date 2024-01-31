using System.Collections.Generic;
using Sales.RabbitMQ.Client.Common.Enums;

namespace Sales.Domain.Enums;

public static class CountsQueueType
{
    
    public static Dictionary<CountsType, string> QueueType = new()
    {
        {
            CountsType.Counts, "CreateCounts"
        },
        {
            CountsType.PotentialCounts, "CreatePotentialCounts"
        }
    };
       
}
