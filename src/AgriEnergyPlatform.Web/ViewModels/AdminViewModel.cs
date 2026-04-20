using AgriEnergyPlatform.Web.Models;

namespace AgriEnergyPlatform.Web.ViewModels;

public class AdminViewModel
{
    public List<PlatformUser> Admins { get; set; } = new();
}