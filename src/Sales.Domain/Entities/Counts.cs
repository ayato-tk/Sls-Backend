
namespace Sales.Domain.Entities;

public class Counts
{
    public int SessionId { get; set; }

    public int CompaniesCount { get; set; }

    public int CompaniesExtractionCount { get; set; }

    public int Percentage { get; set; }

    public int PartnersCount { get; set; }

    public int PotentialPartners { get; set; }

    public int PhonesCount { get; set; }

    public int PotentialPhones { get; set; }

    public int EmailsCount { get; set; }

    public int PotentialEmails { get; set; }

    public int EmployeeCount { get; set; }

    public int PotentialEmployees { get; set; }

    public double InvoicingSum { get; set; }

    public int PotentialInvoicing { get; set; }
}
