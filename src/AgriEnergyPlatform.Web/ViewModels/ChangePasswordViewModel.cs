using System.ComponentModel.DataAnnotations;

namespace AgriEnergyPlatform.Web.ViewModels;

public class ChangePasswordViewModel
{
    public string OldPassword { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;

    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}