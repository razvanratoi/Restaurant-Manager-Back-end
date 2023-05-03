using RestaurantManager.Models;

namespace RestaurantManager.Observer;

public class Logger : IObserver
{
    public void Update(Order order, int kitchenId)
    {
        using (StreamWriter writer = new StreamWriter("logs.txt", true))
        {
            switch(order.Status){
                case "Pending":
                    writer.WriteLine($"Waiter {order.WaiterId} took an order from table {order.TableNo} at {order.OrderDate}");
                    break;
                case "Finished":
                    writer.WriteLine($"Kitchen staff {kitchenId} finished order {order.Id}");
                    break;
                default: 
                    break;
            }
        }
    }
}
