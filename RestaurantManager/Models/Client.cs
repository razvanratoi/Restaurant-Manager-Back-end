using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantManager.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public double Points { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; set; } = new List<Order>(); 
        public Client(int id, string name, string email, double points)
        {
            Id = id;
            Name = name;
            Email = email;
            Points = points;
        }
    }
}