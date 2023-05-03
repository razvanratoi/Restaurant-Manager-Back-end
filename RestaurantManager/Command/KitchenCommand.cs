using RestaurantManager.Models.Constants;
using RestaurantManager.Observer;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Services;

public class KitchenCommand : IStaffCommand
{
    private readonly IOrderRepo _orderRepo;

    public KitchenCommand(IOrderRepo orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public async void Execute(int kitchenStaff, int orderId)
    {
        var order =  await _orderRepo.GetById(orderId);
        order.Attach(new Logger());
        order.Status = Status.Finished;
        order.Notify(kitchenStaff);
        await _orderRepo.Update(order);
    }
}
