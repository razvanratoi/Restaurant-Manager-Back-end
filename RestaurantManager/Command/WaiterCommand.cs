using RestaurantManager.Models;
using RestaurantManager.Models.Constants;
using RestaurantManager.Observer;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Services;

public class WaiterCommand : IStaffCommand
{
    private readonly IOrderRepo _orderRepo;
    private readonly IClientRepo _clientRepo;

    public WaiterCommand(IOrderRepo orderRepo, IClientRepo clientRepo)
    {
        _orderRepo = orderRepo;
        _clientRepo = clientRepo;
    }

    public async void Execute(int staffId, int orderId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId);
        order.Attach(new EmailSender());
        var client = await _clientRepo.GetByIdAsync(order.ClientId);
        var price = GetOrderPrice(order, client);
        var clientPoints = UpdateLoyaltyPoints(client);
        order.Total = price;
        order.Status = Status.Paid;
        order.Notify(-1);
        await _orderRepo.Update(order);
    }

    public double GetOrderPrice(Order order, Client client)
    {
        var price = 0.0;
        foreach (var product in order.Products)
        {
            price += product.Price;
        }

        var newPrice = price - (price * (client.Points / 10));
        return Math.Round(newPrice, 2);
    }

    public async Task UpdateLoyaltyPoints(Client client)
    {
        var clientLoyaltyPoints = client.Points;

        if (clientLoyaltyPoints < 5)
            clientLoyaltyPoints += 0.1;

        await _clientRepo.Update(client);
    }
}
