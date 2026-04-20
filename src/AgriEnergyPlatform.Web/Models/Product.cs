using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AgriEnergyPlatform.Web.Models;

public class Product
{
    [Key] 
    public int Id { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "The name must be 200 characters or less.")]
    public string? Name { get; set; }
    
    [Required] 
    [StringLength(3000, ErrorMessage = "The description must be 3000 characters or less.")]
    [Column(TypeName = "TEXT")]
    public string? Description { get; set; }
    
    [Required]
    public DateOnly ProductionDate { get; set; }

    public int Quantity { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    [Required]
    public bool IsWeight { get; set; } 
    
    [Required]
    [ForeignKey("PlatformUser")]
    [StringLength(450, ErrorMessage = "The userId must be 450 characters or less.")]
    public required string? PlatformUserId { get; set; }
    public virtual PlatformUser? PlatformUser { get; init; }
    

    [ForeignKey("CategoryProductId")]
    public int? CategoryProductId { get; set; }
    public virtual CategoryProduct? CategoryProduct { get; set; }

    [Required]
    [ForeignKey("ProductImage")]
    public int ProductImageId { get; set; }
    public virtual ICollection<ProductImage>? ProductImages { get; set; }
}