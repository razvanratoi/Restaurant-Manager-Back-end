using RestaurantManager.Models;

namespace RestaurantManager.Observer;

public interface IObserver {
    void Update(Order order, int id);
}
