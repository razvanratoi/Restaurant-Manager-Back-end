using RestaurantManager.Data;
using RestaurantManager.Models;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Repositories
{
    public class ClientRepo : GenericRepo<Client>, IClientRepo
    {
        public DataContext _context { get; }
        public ClientRepo(DataContext context) : base(context)
        {
            _context = context;
        }

        public virtual Client? GetClient(int id){
            return GetByIdAsync(id).Result;
        }
    }
}