using BookMySeatApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
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
    [Test]
    public void TestGetOrders()
    {
        Mock<OrderRepo> orderRepo = new Mock<OrderRepo>(null);
        Mock<ClientRepo> clientRepo = new Mock<ClientRepo>(null);
        Mock<UserRepo> userRepo = new Mock<UserRepo>(null);
        Mock<ProductRepo> productRepo = new Mock<ProductRepo>(null);
        UserService userService = new UserService(userRepo.Object, new PasswordService(), null);
        OrderService orderService = new OrderService(orderRepo.Object, clientRepo.Object, productRepo.Object, userService);

        Order[] orders = {  new Order(1, 1, 1, 1, Status.Pending, new List<Product>() { new Product(1, "Coca Cola", "Coca Cola", 5.0), new Product(1, "Carbonara", "Pasta", 7.0) }),
                    new Order(2, 1, 1, 1, Status.Pending, new List<Product>() { new Product(1, "Coca Cola", "Coca Cola", 5.0), new Product(1, "Carbonara", "Pasta", 7.0) }),
                    new Order(3, 1, 1, 1, Status.Pending, new List<Product>() { new Product(1, "Coca Cola", "Coca Cola", 5.0), new Product(1, "Carbonara", "Pasta", 7.0) }) };
      
        orderRepo.Setup(x => x.GetAll()).ReturnsAsync((List<Order>)orders.ToList());

        var ordersTest = orderService.GetOrders();
        Assert.AreEqual(3, ordersTest.Count);
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
        Mock<OrderRepo> orderRepo = new Mock<OrderRepo>(null);
        Mock<ClientRepo> clientRepo = new Mock<ClientRepo>(null);
        Mock<UserRepo> userRepo = new Mock<UserRepo>(null);
        Mock<ProductRepo> productRepo = new Mock<ProductRepo>(null);
        UserService userService = new UserService(userRepo.Object, new PasswordService(), null);
        OrderService orderService = new OrderService(orderRepo.Object, clientRepo.Object, productRepo.Object, userService);

        clientRepo.Setup(x => x.GetClient(1)).Returns(new Client(1, "Razvan frumuselu", "razvi.ratoi@icloud.com", 5));

        var order = new Order();
        order.ClientId = 7;
        order.Client = new Client(1, "Razvan frumuselu", "razvi.ratoi@icloud.com", 5);
        order.WaiterId = 4;
        order.Status = Status.Pending;
        order.TableNo = 27;
        order.Products = new List<Product>();
        order.Products.Add(new Product(1, "Coca Cola", "Coca Cola", 5.0));
        order.Products.Add(new Product(1, "Carbonara", "Pasta", 7.0));


        WaiterCommand command = new WaiterCommand(orderRepo.Object, clientRepo.Object);

        Assert.AreEqual(6.0d, command.GetOrderPrice(order, order.Client));
    }
}
