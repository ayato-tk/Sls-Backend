using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sales.Domain.Entities.CustomProject;

namespace Sales.Domain.Entities;

public class Filter
{
     [JsonPropertyName("cnaeList")]
    public required List<Cnae> CnaeList { get; set; }

    [JsonPropertyName("companySizeList")]
    public required List<string> CompanySizeList { get; set; }

    [JsonPropertyName("companyRegionList")]
    public required List<string> CompanyRegionList { get; set; }

    [JsonPropertyName("employeeQuantity")]
    public ValueRange? EmployeeQuantity { get; set; }

    [JsonPropertyName("companyInvoicing")]
    public required ValueRange CompanyInvoicing { get; set; }

    [JsonPropertyName("yearsOpeningCompany")]
    public required ValueRange YearsOpeningCompany { get; set; }

}
