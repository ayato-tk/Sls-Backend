namespace Sales.Domain.Entities.Dashboards.Items.Invoicing;

public class InvoicingItem : BaseItem
{
    public int MinimumInvoicing { get; set; }

    public int MaximumInvoicing { get; set; }
}
