namespace RestaurantManager.Services;

public interface IStaffCommand
{
    void Execute(int staffId, int orderId);
}
