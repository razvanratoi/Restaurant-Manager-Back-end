using System.ComponentModel.DataAnnotations;
using RestaurantManager.Models.Constants;

namespace RestaurantManager.DTOs;

public class UserDto
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Role { get; set; } = string.Empty;
}
