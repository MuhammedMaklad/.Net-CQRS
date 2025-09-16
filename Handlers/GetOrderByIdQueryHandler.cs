using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderApi.Contracts;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Models;
using OrderApi.Queries;

namespace OrderApi.Handlers;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
  private readonly ReadDbContext context;
  private readonly IMapper mapper;
  public GetOrderByIdQueryHandler(ReadDbContext context, IMapper mapper)
  {
    this.context = context;
    this.mapper = mapper;
  }
  public static async Task<Order?> Handle(GetOrderByIdQuery query, AppDbContext context)
  {
    return await context.Orders.FirstOrDefaultAsync(x => x.Id == query.OrderId);
  }

  public async Task<OrderDto?> HandlerAsync(GetOrderByIdQuery query)
  {
    var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == query.OrderId);
    if (order is null)
      return null;
    return mapper.Map<OrderDto>(order);
  }
}
