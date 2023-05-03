using RestaurantManager.Models;

namespace RestaurantManager.Repositories.Interfaces
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<User?> FindByUsername(string username);
    }
}