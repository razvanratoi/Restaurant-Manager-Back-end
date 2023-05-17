using RestaurantManager.Models;
using RestaurantManager.Repositories;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Services;

public class ProductService
{
    private readonly IProductRepo _productRepo;
    private readonly IGenericRepo<Category> _categoryRepo;
    public ProductService(IProductRepo productRepo, IGenericRepo<Category> categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<Product> CreateProduct(Product product)
    {
        var cat = _categoryRepo.GetByIdAsync(product.CategoryId).Result;
        product.Category = cat;
        await _productRepo.Add(product);
        return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        var prod = await _productRepo.GetByIdAsync(product.Id);
        product.Id = prod.Id;
        await _productRepo.Update(product);
        return product;
    }

    public async Task<Product> DeleteProduct(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        await _productRepo.Delete(product);
        return product;
    }

    public async Task<Product?> GetProduct(int id)
    {
        return await _productRepo.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _productRepo.GetAll();
    }
}
