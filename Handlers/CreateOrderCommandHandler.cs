using System.Reflection.Metadata;
using AutoMapper;
using OrderApi.Commands;
using OrderApi.Contracts;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Models;

namespace OrderApi.Handlers;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
{
  private readonly IMapper mapper;
  private readonly AppDbContext context;
  public CreateOrderCommandHandler(AppDbContext context, IMapper mapper)
  {
    this.mapper = mapper;
    this.context = context;
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
    
    return mapper.Map<OrderDto>(order);
  }
}
