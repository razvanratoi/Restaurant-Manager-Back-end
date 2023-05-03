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

        public override async Task<Order?> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Products)
                    .ThenInclude(p => p.Category)
                .FirstAsync(o => o.Id == id);
            return order;
        }
    }
}