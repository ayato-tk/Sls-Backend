using System.Collections.Generic;

namespace Sales.Domain.Enums;

public static class FileOrigin
{
    
    public static Dictionary<ProjectType, string> Origin = new()
    {
        {
            ProjectType.Icp, "ICP"
        },
        {
            ProjectType.Custom, "CUSTOM"
        }
    };
       
}
