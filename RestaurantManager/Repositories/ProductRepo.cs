using RestaurantManager.Data;
using RestaurantManager.Models;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Repositories
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public DataContext _context { get; }
        public ProductRepo(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}