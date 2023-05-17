using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.DTOs;
using RestaurantManager.Models;
using RestaurantManager.Models.Constants;
using RestaurantManager.Services;

namespace RestaurantManager.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Waiter)]
    public IActionResult GetOrders()
    {
        var orders = _orderService.GetOrders();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Roles.Waiter)]
    public IActionResult GetOrder(int id)
    {
        var order = _orderService.GetOrder(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Waiter)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto order)
    {
        try
        {
            await _orderService.CreateOrder(order);
            return Created(nameof(CreateOrder), order);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Waiter)]
    public IActionResult UpdateOrder(int id, Order order)
    {
        try
        {
            _orderService.UpdateOrder(order);
            return Ok($"Order updated successfully: {order}");
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{id}/addProduct")]
    [Authorize(Roles = Roles.Waiter)]
    public async Task<IActionResult> AddProductToOrderAsync([FromRoute] int id, [FromBody] int productId)
    {
        try
        {
            await _orderService.AddProductToOrder(id, productId);
            var order =  _orderService.GetOrder(id);
            return Ok(order);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{id}/pay")]
    [Authorize(Roles = Roles.Waiter)]
    public  IActionResult PayOrderAsync(int id)
    {
        try
        {
            _orderService.PayOrder(id);
            return Ok("Order paid successfully");
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Waiter)]
    public async Task<IActionResult> DeleteOrderAsync(int id)
    {
        try
        {
            var order = _orderService.GetOrder(id);
            await _orderService.DeleteOrder(order);
            return Ok("Order deleted successfully");
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{id}/finish")]
    [Authorize(Roles = Roles.Kitchen)]
    public async Task<IActionResult> FinishOrderAsync(int id)
    {
        try
        {
            await _orderService.FinishOrder(id, HttpContext);
            return Ok("Order finished successfully");
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet("{id}/products")]
    [Authorize(Roles = Roles.Waiter)]
    public IActionResult GetProductsOfOrder(int id)
    {
        try
        {
            var products = _orderService.GetProductsOfOrder(id);
            return Ok(products);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}
