using MediatR;
using OrderApi.DTOs;

namespace OrderApi.Commands;

public record CreateOrderCommand(string FirstName, string LastName, string Status, decimal TotalCost) : IRequest<OrderDto>;