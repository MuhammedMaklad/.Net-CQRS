namespace OrderApi.Events;
public record OrderCreatedEvent(int Id, string FirstName, string LastName, string Status, decimal TotalCost, DateTime CreatedAt);

