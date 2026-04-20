using System.ComponentModel.DataAnnotations;

namespace AgriEnergyPlatform.Web.ViewModels;

public class ProductViewModel()
{
    [Required]
    [StringLength(300, ErrorMessage = "The name must be 300 characters or less.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(3000, ErrorMessage = "The description must be 3000 characters or less.")]
    public string Description { get; init; } = string.Empty;

    [Required]
    public DateTime ProductionDate { get; init; }

    public int Quantity { get; init; }
    
    [Required]
    public bool IsWeight { get; set; }

    [Required]
    public string Price { get; set; } = string.Empty;

    [Required]
    public int CategoryProductId { get; set; }
    
    public string? NewCategoryName { get; set; }

    [Required(ErrorMessage = "Please upload at least one image.")]
    public List<IFormFile> Images { get; init; } = new();
}