using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgriEnergyPlatform.Web.Models;

public class Blog
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "The title must be 50 characters or less.")]
    public required string Title { get; set; }
    
    [Required]
    [StringLength(300, ErrorMessage = "The URL must be 150 characters or less.")]
    public required string BlogImageUrl { get; set; }

    [StringLength(50, ErrorMessage = "The Subtitle must be 50 characters or less.")]
    public string? Subtitle { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "The description must be 300 characters or less.")]
    public required string Intro { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "The Heading must be 50 characters or less.")]
    public required string Heading1 { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public required string Content1 { get; set; }

    [StringLength(300, ErrorMessage = "The URL must be 150 characters or less.")] 
    public string? Heading1ImageUrl { get; set; }
    
    [StringLength(50, ErrorMessage = "The heading must be 50 characters or less.")]
    public string? Heading2 { get; set; }
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public string? Content2 { get; set; }
    [StringLength(300, ErrorMessage = "The URL must be 150 characters or less.")] 
    public string? Heading2ImageUrl { get; set; }
    
    [StringLength(50, ErrorMessage = "The heading must be 50 characters or less.")]
    public string? Heading3 { get; set; }
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public string? Content3 { get; set; }
    [StringLength(300, ErrorMessage = "The URL must be 150 characters or less.")] 
    public string? Heading3ImageUrl { get; set; }
    
    [StringLength(50, ErrorMessage = "The heading must be 50 characters or less.")]
    public string? Heading4 { get; set; }
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public string? Content4 { get; set; }
    [StringLength(300, ErrorMessage = "The URL must be 150 characters or less.")] 
    public string? Heading4ImageUrl { get; set; }
    
    [StringLength(50, ErrorMessage = "The heading must be 50 characters or less.")]
    public string? Heading5 { get; set; }
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public string? Content5 { get; set; }
    [StringLength(300, ErrorMessage = "The URL must be 150 characters or less.")] 
    public string? Heading5ImageUrl { get; set; }
    
    [Required]
    public DateOnly PublishDate { get; set; }

    public DateOnly UpdateDate { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "The slug must be 200 characters or less.")]
    public required string Slug { get; set; }

    [Required]
    [ForeignKey(nameof(PlatformUser))]
    [StringLength(450, ErrorMessage = "The userId must be 450 characters or less.")]
    public string? PlatformUserId { get; set; }

    public virtual PlatformUser? PlatformUser { get; set; }

    [JsonIgnore]
    public ICollection<BlogCategory>? BlogCategories { get; set; }
}
