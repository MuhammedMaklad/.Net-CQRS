using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApi.Contracts;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Models;
using OrderApi.Queries;

namespace OrderApi.Handlers;

// public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>

{
  private readonly ReadDbContext context;
  private readonly IMapper mapper;
  public GetOrderByIdQueryHandler(ReadDbContext context, IMapper mapper)
  {
    this.context = context;
    this.mapper = mapper;
  }
  public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
  {
    var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId);
    if (order is null)
      return null;
    return mapper.Map<OrderDto>(order);
  }

  public async Task<OrderDto?> HandlerAsync(GetOrderByIdQuery query)
  {
    var order = await context.Orders.
    AsNoTracking()
    .FirstOrDefaultAsync(x => x.Id == query.OrderId);
    if (order is null)
      return null;
    return mapper.Map<OrderDto>(order);
  }
}
