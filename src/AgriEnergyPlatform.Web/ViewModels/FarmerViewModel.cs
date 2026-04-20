namespace AgriEnergyPlatform.Web.ViewModels;

public class FarmerViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public string Id { get; set; } = string.Empty;
    
    public string sortOrder { get; set; } = string.Empty;
}
