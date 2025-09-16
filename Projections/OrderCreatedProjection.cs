using AutoMapper;
using OrderApi.Events;
using OrderApi.Models;

namespace OrderApi.Projections;

public class OrderCreatedProjectionHandler : IEventHandler<OrderCreatedEvent>
{
  private readonly IMapper mapper;
  private readonly ReadDbContext context;

  public OrderCreatedProjectionHandler(IMapper mapper, ReadDbContext context)
  {
    this.mapper = mapper;
    this.context = context;
  }
  public async Task HandleAsync(OrderCreatedEvent evt)
  {
    var orderEntity = mapper.Map<Order>(evt);

    await context.Orders.AddAsync(orderEntity);
    await context.SaveChangesAsync();
  }
}
