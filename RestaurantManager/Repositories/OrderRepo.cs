using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Repositories
{
    public class OrderRepo : GenericRepo<Order>, IOrderRepo
    {
        public DataContext _context { get; }
        public OrderRepo(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Products)
                    .ThenInclude(p => p.Category)
                .ToListAsync();
        }

        public virtual IEnumerable<Order> GetAllAux()
        {
            return GetAll().Result;
        }

        

        public virtual Order? GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }
    }
}