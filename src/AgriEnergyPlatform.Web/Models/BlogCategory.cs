using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgriEnergyPlatform.Web.Models;

public class BlogCategory
{
    [Key] 
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("Blog")]
    [Column(TypeName = "int")]
    public int BlogId { get; init; }
    [JsonIgnore]
    public Blog? Blog { get; init; }
    
    [Required]
    [ForeignKey("CategoryBlog")]
    [Column(TypeName = "int")]
    public int CategoryBlogId { get; init; }
    public CategoryBlog? CategoryBlog { get; init; }
}