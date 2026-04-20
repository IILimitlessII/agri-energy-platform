using AgriEnergyPlatform.Web.Models;

namespace AgriEnergyPlatform.Web.ViewModels;

public class ManageViewModel
{
    public PlatformUser User { get; set; } = new();
    public IList<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    public int ProductsCount { get; set; }
    public int BlogsCount { get; set; }
}