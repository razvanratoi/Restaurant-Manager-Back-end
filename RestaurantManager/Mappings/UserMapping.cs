using AutoMapper;
using RestaurantManager.DTOs;
using RestaurantManager.Models;

namespace RestaurantManager.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}