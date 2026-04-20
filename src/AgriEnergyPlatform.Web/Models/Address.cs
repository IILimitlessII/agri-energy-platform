using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyPlatform.Web.Models;

public class Address
{
    [Key]
    public int AddressId { get; init; }

    [Required]
    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public required string AddressLine1 { get; set; }

    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string? AddressLine2 { get; set; }

    [Required]
    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public required string City { get; set; }

    [Required]
    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public required string Province { get; set; }

    [Required]
    [StringLength(10)]
    [Column(TypeName = "varchar(10)")]
    public required string ZipCode { get; set; }
}