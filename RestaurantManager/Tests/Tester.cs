using BookMySeatApi.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RestaurantManager.Data;
using RestaurantManager.Models;
using RestaurantManager.Models.Constants;
using RestaurantManager.Repositories;
using RestaurantManager.Services;

namespace RestaurantManager.Tests;

[TestFixture]
public class Tester
{
    private readonly OrderService _orderService = new OrderService(new OrderRepo(new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlite("Data source=Restaurant.db").Options)), 
                                                                                 new ClientRepo(new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlite("Data source=Restaurant.db").Options)), 
                                                                                 new ProductRepo(new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlite("Data source=Restaurant.db").Options)), 
                                                                                 new UserService(new UserRepo(new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlite("Data source=Restaurant.db").Options)), 
                                                                                 new PasswordService(), null));

    [Test]
    public void TestGetProductsOfOrder()
    {
        var order = _orderService.GetOrder(1);
        Assert.AreEqual(2, order.Products.Count);
    }

    [Test]
    public void TestGetOrders()
    {
        var orders = _orderService.GetOrders();
        Assert.AreEqual(3, orders.Count);
    }

    [Test]
    public void TestHashPassword()
    {
        var salt = "$2a$10$YGN8JJJo35WWOkIlnrDp0u";
        var password = "0ParolaFoarteGrea@";
        var hasher = new PasswordService();
        var hashedPassword = hasher.HashPassword(password, salt);
        Assert.AreEqual("$2a$10$YGN8JJJo35WWOkIlnrDp0uA5fjo34.ehYMD7bcelbPr76Z1XaUhsy", hashedPassword);
    }

    [Test]
    public void TestGetDiscounts()
    {
        var order = new Order();
        order.ClientId = 7;
        order.Client = new Client(1, "Razvan frumuselu", "razvi.ratoi@icloud.com", 5);
        order.WaiterId = 4;
        order.Status = Status.Pending;
        order.TableNo = 27;
        order.Products = new List<Product>();
        order.Products.Add(new Product(1, "Coca Cola", "Coca Cola", 5.0));
        order.Products.Add(new Product(1, "Carbonara", "Pasta", 7.0));
        WaiterCommand command = new WaiterCommand(null, null);
        Assert.AreEqual(6.0d, command.GetOrderPrice(order, order.Client));
    }
}
