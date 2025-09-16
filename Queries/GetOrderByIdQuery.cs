using MediatR;
using OrderApi.DTOs;

namespace OrderApi.Queries;

public record GetOrderByIdQuery(int OrderId): IRequest<OrderDto?>;

