using System.ComponentModel.DataAnnotations;

namespace AgriEnergyPlatform.Web.Models;

public class CategoryBlog
{
    [Key] 
    public int Id { get; init; }

    [Required]
    [StringLength(50, ErrorMessage = "The name must be 50 characters or less.")]
    public string? Name { get; init; }

    [Required]
    [StringLength(500, ErrorMessage = "The description must be 500 characters or less.")]
    public string? Description { get; init; }
    
    // Many-to-Many relationship with Blog through BlogCategory
    public ICollection<BlogCategory>? Blogs { get; init; }
}