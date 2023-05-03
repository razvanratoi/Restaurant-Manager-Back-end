using AutoMapper;
using RestaurantManager.DTOs;
using RestaurantManager.Models;
using RestaurantManager.Models.Constants;
using RestaurantManager.Observer;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Services;

public class OrderService
{
    private readonly IOrderRepo _orderRepo;
    private readonly IClientRepo _clientRepo;
    private readonly IProductRepo _productRepo;
    private readonly UserService _userService;

    public OrderService(IOrderRepo orderRepo, IClientRepo clientRepo, IProductRepo productRepo, UserService userService)
    {
        _orderRepo = orderRepo;
        _clientRepo = clientRepo;
        _productRepo = productRepo;
        _userService = userService;
    }

    public async Task<Order> CreateOrder(OrderDto orderDto)
    {
        var client = await _clientRepo.GetById(orderDto.ClientId);
        var products = new List<Product>();

        foreach (var productId in orderDto.Products)
        {
            var product = await _productRepo.GetById(productId);
            products.Add(product);
        }

        var order = new Order();
        order.ClientId = orderDto.ClientId;
        order.WaiterId = orderDto.WaiterId;
        order.Products = products;
        order.TableNo = orderDto.TableNo;
        order.Client = client;
        order.Products = products;

        order.Attach(new Logger());
        order.Attach(new EmailSender());
        order.Notify(-1);
        await _orderRepo.Add(order);
        return order;
    }

    public List<Order> GetOrders()
    {
        return (List<Order>)_orderRepo.GetAll().Result;
    }

    public Order GetOrder(int id)
    {
        return _orderRepo.GetById(id).Result;
    }

    public async Task<Order> UpdateOrder(Order order)
    {
        var client = await _clientRepo.GetById(order.ClientId);
        var products = new List<Product>();
        foreach (var product in order.Products)
            products.Add(await _productRepo.GetById(product.Id));

        var newOrder = new Order
        {
            Client = client,
            Products = new List<Product>()
        };

        foreach (var product in products)
        {
            newOrder.Products.Add(product);
        }

        await _orderRepo.Update(newOrder);
        return newOrder;
    }

    public async Task<bool> DeleteOrder(Order order)
    {
        return await _orderRepo.Delete(order);
    }

    public List<Order> GetOrdersByClient(int clientId)
    {
        var orders = GetOrders();
        var clientOrders = new List<Order>();
        foreach (var order in orders)
        {
            if (order.ClientId == clientId)
                clientOrders.Add(order);
        }

        return clientOrders;
    }

    public async Task<Order> AddProductToOrder(int orderId, int productId)
    {
        var order = await _orderRepo.GetById(orderId);
        order.Status = Status.Pending;
        var product = await _productRepo.GetById(productId);
        order.Products.Add(product);
        await _orderRepo.Update(order);
        return order;
    }

    public List<Product> GetProductsOfOrder(int orderId)
    {
        var order = _orderRepo.GetById(orderId).Result;
        return order.Products;
    }

    public void PayOrder(int orderId)
    {
        IStaffCommand command = new WaiterCommand(_orderRepo, _clientRepo);
        command.Execute(-1, orderId);
    }

    public async Task FinishOrder(int orderId, HttpContext httpContext)
    {
        IStaffCommand command = new KitchenCommand(_orderRepo);
        var kitchenStaff = await _userService.GetUserFromToken(httpContext);
        command.Execute(kitchenStaff.Id, orderId);
    }
}
