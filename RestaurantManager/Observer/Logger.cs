using RestaurantManager.Models;

namespace RestaurantManager.Observer;

public class Logger : IObserver
{
    public void Update(Order order, int kitchenId)
    {

        switch (order.Status)
        {
            case "Pending":
                using (StreamWriter writer = new StreamWriter("waiter_logs.txt", true))
                    writer.WriteLine($"Waiter {order.WaiterId} took an order from table {order.TableNo} at {order.OrderDate}");
                break;
            case "Finished":
                using (StreamWriter writer = new StreamWriter("kitchen_logs.txt", true))
                    writer.WriteLine($"Kitchen staff {kitchenId} finished order {order.Id}");
                break;
            default:
                break;
        }
    }
}

