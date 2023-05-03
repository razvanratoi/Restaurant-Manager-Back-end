using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantManager.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
        
    }
}