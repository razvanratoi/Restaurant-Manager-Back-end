using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantManager.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public Product(int id, string name, string description, double price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
        
    }
}