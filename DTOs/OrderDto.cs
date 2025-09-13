namespace OrderApi.DTOs;

public record OrderDto
{
  public int Id { get; init; }
  public required string FirstName { get; init; }
  public required string LastName { get; init; }
  public required string Status { get; init; }
  public DateTime CreatedAt { get; init; }
  public Decimal TotalCost { get; init; }
}


// public record OrderDto1 (int Id, string FirstName, string LastName, string Status, DateTime CreatedAt);
