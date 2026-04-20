using System.ComponentModel.DataAnnotations;

namespace AgriEnergyPlatform.Web.ViewModels;

public class FarmerRegisterViewModel()
{

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; init; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address Line 1 is required.")]
    public string AddressLine1 { get; init; } = string.Empty;

    public string AddressLine2 { get; init; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    public string City { get; init; } = string.Empty;

    [Required(ErrorMessage = "Province is required.")]
    public string Province { get; init; } = string.Empty;

    [Required(ErrorMessage = "Zip Code is required.")]
    public string ZipCode { get; init; } = string.Empty;

    public string OptionalAddressLine1 { get; init; } = string.Empty;
    public string OptionalAddressLine2 { get; init; } = string.Empty;
    public string OptionalCity { get; init; } = string.Empty;
    public string OptionalProvince { get; init; } = string.Empty;
    public string OptionalZipCode { get; init; } = string.Empty;
}

