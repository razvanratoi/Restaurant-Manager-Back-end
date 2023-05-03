using System.ComponentModel.DataAnnotations;
using RestaurantManager.Observer;

namespace RestaurantManager.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public int WaiterId { get; set; }
        public int TableNo { get; set; }
        public string Status { get; set; } = "Pending";
        public double Total {get; set; } = 0.0;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<Product> Products { get; set; } = new List<Product>();
        private List<IObserver> observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify(int id)
        {
            foreach (IObserver observer in observers)
            {
                observer.Update(this, id);
            }
        }

    }
}