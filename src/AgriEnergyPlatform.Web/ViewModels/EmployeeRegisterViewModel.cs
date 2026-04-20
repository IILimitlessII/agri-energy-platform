using System.ComponentModel.DataAnnotations;

namespace AgriEnergyPlatform.Web.ViewModels;

public class EmployeeRegisterViewModel(
    bool isBlogger,
    string zipCode,
    string? optionalAddressLine1,
    string? optionalAddressLine2,
    string? optionalCity,
    string? optionalProvince,
    string? optionalZipCode,
    string province,
    string city,
    string addressLine2,
    string addressLine1,
    string password,
    string name,
    string email,
    string username,
    string confirmPassword)
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; } = username;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; } = email;

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; } = name;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; } = password;

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = confirmPassword;

    [Required(ErrorMessage = "Address Line 1 is required.")]
    public string AddressLine1 { get; } = addressLine1;

    public string? AddressLine2 { get; } = addressLine2;

    [Required(ErrorMessage = "City is required.")]
    public string City { get; } = city;

    [Required(ErrorMessage = "Province is required.")]
    public string Province { get; } = province;

    [Required(ErrorMessage = "Zip Code is required.")]
    public string ZipCode { get; } = zipCode;

    public string? OptionalAddressLine1 { get; } = optionalAddressLine1;
    public string? OptionalAddressLine2 { get; } = optionalAddressLine2;
    public string? OptionalCity { get; } = optionalCity;
    public string? OptionalProvince { get; } = optionalProvince;
    public string? OptionalZipCode { get; } = optionalZipCode;

    public bool IsBlogger { get; } = isBlogger;
}