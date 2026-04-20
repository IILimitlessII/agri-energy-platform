using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyPlatform.Web.Models;

public class ProductImage
{
    [Key] 
    public int Id { get; set; }
    
    // Foreign key property
    [Required]
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "The Url must be 200 characters or less.")]
    public string? Url { get; set; }
}