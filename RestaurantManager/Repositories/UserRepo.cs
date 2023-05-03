using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Repositories
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        public DataContext _context { get; }
        public UserRepo(DataContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<User?> FindByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}