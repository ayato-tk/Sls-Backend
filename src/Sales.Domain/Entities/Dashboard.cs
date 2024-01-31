


using System.Collections.Generic;
using Sales.Domain.Entities.Dashboards.Items.Cnae;
using Sales.Domain.Entities.Dashboards.Items.Company;
using Sales.Domain.Entities.Dashboards.Items.Employee;
using Sales.Domain.Entities.Dashboards.Items.Invoicing;
using Sales.Domain.Entities.Dashboards.Items.OpeningYears;
using Sales.Domain.Entities.Dashboards.Items.Region;


namespace Sales.Domain.Entities;

public class Dashboard
{
    public List<CnaeItem> Cnae { get; set; } = [];
    public List<CnaeDivisionItem> CnaeDivision { get; set; } = [];
    public List<CnaeSectionItem> CnaeSection { get; set; } = [];
    public List<InvoicingItem> Invoicing { get; set; } = [];
    public List<EmployeeQuantityItem> EmployeeQuantity { get; set; } = [];
    public List<CompanySizeItem> CompanySize { get; set; } = [];
    public List<RegionItem> Region { get; set; } = [];
    public List<RegionAbreviationItem> RegionAbreviation { get; set; } = [];
    public List<OpeningYearsItem> OpeningYears { get; set; } = [];

    public InvoicingMinMaxItem Invoicing_MinMax { get; set; } = new InvoicingMinMaxItem();
    public EmployeeQuantityMinMaxItem EmployeeQuantity_MinMax { get; set; } = new EmployeeQuantityMinMaxItem();
    public OpeningYearsMinMaxItem OpeningYears_MinMax { get; set; } = new OpeningYearsMinMaxItem();
}
