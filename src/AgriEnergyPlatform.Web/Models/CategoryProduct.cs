using System.ComponentModel.DataAnnotations;

namespace AgriEnergyPlatform.Web.Models;

public class CategoryProduct
{
    [Key] 
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "The name must be 50 characters or less.")]
    public string? Name { get; set; }
    
    public ICollection<Product>? Products { get; set; }
}