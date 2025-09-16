using System.Reflection.Metadata;
using AutoMapper;
using FluentValidation;
using OrderApi.Commands;
using OrderApi.Contracts;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Events;
using OrderApi.Models;

namespace OrderApi.Handlers;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
{
  private readonly IMapper mapper;
  private readonly AppDbContext context;
  // private readonly IValidator<CreateOrderCommandValidator> validator;
  private readonly IEventPublisher eventPublisher;
  public CreateOrderCommandHandler(
    AppDbContext context,
     IMapper mapper,
     IEventPublisher eventPublisher)
  {
    this.mapper = mapper;
    this.context = context;
    // this.validator = validator;
    this.eventPublisher = eventPublisher;
  }
  public static async Task<Order> Handle(CreateOrderCommand command, AppDbContext context)
  {
    var order = new Order()
    {
      FirstName = command.FirstName,
      LastName = command.LastName,
      TotalCost = command.TotalCost,
      Status = command.Status,
      CreatedAt = DateTime.Now
    };
    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();

    return order;
  }

  public async Task<OrderDto?> HandlerAsync(CreateOrderCommand command)
  {
    var order = mapper.Map<Order>(command);
    if (order is null)
      return null;

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();

    // TODO:
    var orderCreatedEvent = new OrderCreatedEvent(order.Id, order.FirstName, order.LastName, order.TotalCost);
    await eventPublisher.PublishAsync(orderCreatedEvent);

    return mapper.Map<OrderDto>(order);
  }
}
