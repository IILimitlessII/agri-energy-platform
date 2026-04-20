using System.ComponentModel.DataAnnotations;

namespace AgriEnergyPlatform.Web.Models;

public class BlogCreateDto
{
    [Required]
    [StringLength(50, ErrorMessage = "The Title must be 50 characters or less.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    public IFormFile BlogImage { get; set; } = default!;

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

    
    [StringLength(50, ErrorMessage = "The heading must be 50 characters or less.")]
    public string? Heading2 { get; set; }
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public string? Content2 { get; set; }
    
    [StringLength(50, ErrorMessage = "The heading must be 50 characters or less.")]
    public string? Heading3 { get; set; }
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public string? Content3 { get; set; }
    
    [StringLength(50, ErrorMessage = "The heading must be 50 characters or less.")]
    public string? Heading4 { get; set; }
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public string? Content4 { get; set; }
    
    [StringLength(50, ErrorMessage = "The heading must be 50 characters or less.")]
    public string? Heading5 { get; set; }
    [StringLength(500, ErrorMessage = "The Content must be 500 characters or less.")]
    public string? Content5 { get; set; }

    public IFormFile? Heading1Image { get; set; }

    public IFormFile? Heading2Image { get; set; }
    
    public IFormFile? Heading3Image { get; set; }

    public IFormFile? Heading4Image { get; set; }

    public IFormFile? Heading5Image { get; set; }
    
    [Required(ErrorMessage = "Please Select Category")]
    public int CategoryId { get; set; }
}
