namespace Sales.Domain.Entities.Dashboards;

public abstract class BaseItem
{

  public int SessionId { get; set; }

  public string Session { get; set; } = "";

  public int Sample { get; set; }

  public int Counter { get; set; }

  public float Percent { get; set; }
}
