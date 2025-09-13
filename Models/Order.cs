namespace OrderApi.Models;

public class Order
{
  public int Id { get; set; }
  public string FirstName { get; set; } = string.Empty;
  public required string LastName { get; set; }
  public required string Status { get; set; }
  public decimal TotalCost { get; set; }
  public DateTime CreatedAt { get; set; }
}
