using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.Models;
using RestaurantManager.Models.Constants;
using RestaurantManager.Services;

namespace RestaurantManager.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [Authorize(Roles = $"{Roles.Manager}, {Roles.Waiter}")]
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = _productService.GetProducts();
        return Ok(products.Result);
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var product = _productService.GetProduct(id);
        if (product == null)
            return NotFound();
        return Ok(product.Result);
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        try
        {
            await _productService.CreateProduct(product);
            return Created(nameof(CreateProduct), product);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductAsync(int id, Product product)
    {
        try
        {
            await _productService.UpdateProduct(product);
            return Ok($"Product updated successfully: {product}");
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            _productService.DeleteProduct(id);
            return Ok($"Product deleted successfully: {id}");
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}
