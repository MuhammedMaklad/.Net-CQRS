namespace OrderApi.Events;
public record OrderCreatedEvent(int OrderId, string FirstName, string LastName, decimal TotalCost);
