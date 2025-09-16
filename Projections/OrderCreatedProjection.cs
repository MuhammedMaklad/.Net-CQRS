using AutoMapper;
using MediatR;
using OrderApi.Events;
using OrderApi.Models;

namespace OrderApi.Projections;

// public class OrderCreatedProjectionHandler : IEventHandler<OrderCreatedEvent>
public class OrderCreatedProjectionHandler : INotificationHandler<OrderCreatedEvent>
{
  private readonly IMapper mapper;
  private readonly ReadDbContext context;

  public OrderCreatedProjectionHandler(IMapper mapper, ReadDbContext context)
  {
    this.mapper = mapper;
    this.context = context;
  }

  public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
  {
    var orderEntity = mapper.Map<Order>(notification);

    await context.Orders.AddAsync(orderEntity, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);
  }

  public async Task HandleAsync(OrderCreatedEvent evt)
  {
    var orderEntity = mapper.Map<Order>(evt);

    await context.Orders.AddAsync(orderEntity);
    await context.SaveChangesAsync();
  }
}
