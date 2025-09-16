using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderApi.Contracts;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Queries;

namespace OrderApi;

public class GetOrderSummaryQueryHandler : IQueryHandler<GetOrderSummaryQuery, List<OrderSummaryDto>>
{
  
  private readonly IMapper mapper ;
  private readonly WriteDbContext context;
  public GetOrderSummaryQueryHandler(IMapper mapper, WriteDbContext context)
  {
    this.mapper = mapper;
    this.context = context;
  }
  public async Task<List<OrderSummaryDto>?> HandlerAsync(GetOrderSummaryQuery query)
  {
    var orders = await context.Orders.Select(x => new OrderSummaryDto(x.Id, x.FirstName + " " + x.LastName, x.Status, x.TotalCost)).ToListAsync();
    return orders;
  }
}
