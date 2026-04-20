using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyPlatform.Web.Models;

public class PlatformUser: IdentityUser
{
    [StringLength(100)]
    [MaxLength(100)]
    [Required]
    public string? Name { get; set; }  
    
    public ICollection<UserAddress>? Addresses { get; set; }
}