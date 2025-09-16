using System.Reflection.Metadata;
using AutoMapper;
using FluentValidation;
using MediatR;
using OrderApi.Commands;
using OrderApi.Contracts;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Events;
using OrderApi.Models;

namespace OrderApi.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
  private readonly IMapper mapper;
  private readonly WriteDbContext context;
  // private readonly IValidator<CreateOrderCommandValidator> validator;
  private readonly IEventPublisher eventPublisher;
  private readonly IMediator mediator;
  public CreateOrderCommandHandler(
    WriteDbContext context,
     IMapper mapper,
     //  IEventPublisher eventPublisher,
     //  IEventHandler<OrderCreatedEvent> eventHandler,
     IMediator mediator
     )
  {
    this.mapper = mapper;
    this.context = context;
    // this.validator = validator;
    // this.eventPublisher = eventPublisher;
    this.mediator = mediator;
  }
  // public static async Task<Order> Handle(CreateOrderCommand command, AppDbContext context)
  // {
  //   var order = new Order()
  //   {
  //     FirstName = command.FirstName,
  //     LastName = command.LastName,
  //     TotalCost = command.TotalCost,
  //     Status = command.Status,
  //     CreatedAt = DateTime.Now
  //   };
  //   await context.Orders.AddAsync(order);
  //   await context.SaveChangesAsync();

  //   return order;
  // }

  public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
  {
    var order = mapper.Map<Order>(request);

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();

    // TODO:
    var orderCreatedEvent = mapper.Map<OrderCreatedEvent>(order);
    // await eventPublisher.PublishAsync<OrderCreatedEvent>(orderCreatedEvent);
    await mediator.Publish(orderCreatedEvent);

    return mapper.Map<OrderDto>(order);
  }

  public async Task<OrderDto?> HandlerAsync(CreateOrderCommand command)
  {
    var order = mapper.Map<Order>(command);
    if (order is null)
      return null;

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();

    // TODO:
    var orderCreatedEvent = mapper.Map<OrderCreatedEvent>(order);
    await eventPublisher.PublishAsync<OrderCreatedEvent>(orderCreatedEvent);

    return mapper.Map<OrderDto>(order);
  }
}
