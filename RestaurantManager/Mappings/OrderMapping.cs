using AutoMapper;
using RestaurantManager.DTOs;
using RestaurantManager.Models;

namespace RestaurantManager.Mappings;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();
    }
}
