using AutoMapper;
using OrderApi.Commands;
using OrderApi.DTOs;
using OrderApi.Models;
namespace OrderApi;

public class OrderProfile : Profile
{
  public OrderProfile()
  {
    CreateMap<Order, OrderDto>().ReverseMap();
    CreateMap<Order, CreateOrderCommand>().ReverseMap();
  }
}
