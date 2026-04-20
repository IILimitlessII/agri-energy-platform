using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyPlatform.Web.Models;

public class UserAddress
{
    [Key]
    public int UserAddressId { get; init; }
    
    [Required]
    [ForeignKey("PlatformUser")]
    [StringLength(450, ErrorMessage = "The userId must be 450 characters or less.")]
    public required string PlatformUserId { get; init; }
    public PlatformUser? PlatformUser { get; init; }
    
    [Required]
    [ForeignKey("Address")]
    [Column(TypeName = "int")]
    public int AddressId { get; init; }
    public Address? Address { get; set; }
}