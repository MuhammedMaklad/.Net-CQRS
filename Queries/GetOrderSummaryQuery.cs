using MediatR;
using OrderApi.DTOs;

namespace OrderApi.Queries;

public record GetOrderSummaryQuery():IRequest<List<OrderSummaryDto>>;
