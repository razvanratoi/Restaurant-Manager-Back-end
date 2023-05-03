namespace RestaurantManager.DTOs;

public class OrderDto
{
    public int ClientId { get; set; }
    public int WaiterId { get; set; }
    public int TableNo { get; set; }
     public ICollection<int>? Products { get; set; }
}
