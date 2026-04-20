using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using AgriEnergyPlatform.Web.Database;
using AgriEnergyPlatform.Web.Models;
using AgriEnergyPlatform.Web.ViewModels;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyPlatform.Web.Controllers;

public class AccountController(
    SignInManager<PlatformUser> signInManager,
    UserManager<PlatformUser> userManager,
    ApplicationDbContext context)
    : Controller
{
    // GET : Account/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password)
    {
        // Attempt to sign in the user using ASP.NET Core Identity
        var result = await signInManager.PasswordSignInAsync(username, password, false, false);

        if (result.Succeeded)
        {
            // Redirect to a secure area or homepage after successful login
            return RedirectToAction("Index", "Home");
        }

        // If login fails, redisplay the login form with an error message
        ViewBag.ErrorMessage = "Invalid username or password.";
        return View();
    }


    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
    
    
    
    // GET : Account/FarmerRegister
    [Authorize(Policy = "EmployeeOnly")]
    public IActionResult FarmerRegister()
    {
        return View();
    }

    // POST : Account/FarmerRegister
    [HttpPost]
    [Authorize(Policy = "EmployeeOnly")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FarmerRegister(FarmerRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Create a new user object and populate its properties
            var newFarmer = new PlatformUser
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = true
            };

            // Add the user to the database
            var result = await userManager.CreateAsync(newFarmer, model.Password);

            if (result.Succeeded)
            {
                // Add the Farmer claim to the user
                await userManager.AddClaimAsync(newFarmer, new Claim(ClaimTypes.Role, "Farmer"));

                // Add the required address
                var address = new Address
                {
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    Province = model.Province,
                    ZipCode = model.ZipCode
                };
                
                context.Addresses.Add(address);
                await context.SaveChangesAsync();

                var userAddress = new UserAddress
                {
                    PlatformUserId = newFarmer.Id,
                    AddressId = address.AddressId
                };
                
                context.UserAddresses.Add(userAddress);
                await context.SaveChangesAsync();

                // Add the optional address if provided
                if (!string.IsNullOrEmpty(model.OptionalAddressLine1) && !string.IsNullOrEmpty(model.OptionalCity) &&
                    !string.IsNullOrEmpty(model.OptionalProvince) && !string.IsNullOrEmpty(model.OptionalZipCode))
                {
                    var optionalAddress = new Address
                    {
                        AddressLine1 = model.OptionalAddressLine1,
                        AddressLine2 = model.OptionalAddressLine2,
                        City = model.OptionalCity,
                        Province = model.OptionalProvince,
                        ZipCode = model.OptionalZipCode
                    };
                    context.Addresses.Add(optionalAddress);
                    await context.SaveChangesAsync();

                    var optionalUserAddress = new UserAddress
                    {
                        PlatformUserId = newFarmer.Id,
                        AddressId = optionalAddress.AddressId
                    };
                    
                    context.UserAddresses.Add(optionalUserAddress);
                    await context.SaveChangesAsync();
                }

                // Redirect to a success page
                return RedirectToAction("EmployeeRegisterSuccess");
            }
            else
            {
                // If user creation fails, display errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }
        catch (Exception)
        {
            // Log the exception or handle it as needed
            ModelState.AddModelError(string.Empty,
                "An error occurred while processing your request. Please try again later.");
            return View(model);
        }
    }
    
    
    // GET : Account/EnthusiastRegister
    [Authorize(Policy = "EmployeeOnly")]
    public IActionResult EnthusiastRegister()
    {
        return View();
    }

    // POST : Account/EnthusiastRegister
    [HttpPost]
    [Authorize(Policy = "EmployeeOnly")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnthusiastRegister(FarmerRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Create a new user object and populate its properties
            var newFarmer = new PlatformUser
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = true
            };

            // Add the user to the database
            var result = await userManager.CreateAsync(newFarmer, model.Password);

            if (result.Succeeded)
            {
                // Add the Farmer claim to the user
                await userManager.AddClaimAsync(newFarmer, new Claim(ClaimTypes.Role, "Blogger"));

                // Add the required address
                var address = new Address
                {
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    Province = model.Province,
                    ZipCode = model.ZipCode
                };
                
                context.Addresses.Add(address);
                await context.SaveChangesAsync();

                var userAddress = new UserAddress
                {
                    PlatformUserId = newFarmer.Id,
                    AddressId = address.AddressId
                };
                
                context.UserAddresses.Add(userAddress);
                await context.SaveChangesAsync();

                // Add the optional address if provided
                if (!string.IsNullOrEmpty(model.OptionalAddressLine1) && !string.IsNullOrEmpty(model.OptionalCity) &&
                    !string.IsNullOrEmpty(model.OptionalProvince) && !string.IsNullOrEmpty(model.OptionalZipCode))
                {
                    var optionalAddress = new Address
                    {
                        AddressLine1 = model.OptionalAddressLine1,
                        AddressLine2 = model.OptionalAddressLine2,
                        City = model.OptionalCity,
                        Province = model.OptionalProvince,
                        ZipCode = model.OptionalZipCode
                    };
                    context.Addresses.Add(optionalAddress);
                    await context.SaveChangesAsync();

                    var optionalUserAddress = new UserAddress
                    {
                        PlatformUserId = newFarmer.Id,
                        AddressId = optionalAddress.AddressId
                    };
                    
                    context.UserAddresses.Add(optionalUserAddress);
                    await context.SaveChangesAsync();
                }

                // Redirect to a success page
                return RedirectToAction("EmployeeRegisterSuccess");
            }
            else
            {
                // If user creation fails, display errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }
        catch (Exception)
        {
            // Log the exception or handle it as needed
            ModelState.AddModelError(string.Empty,
                "An error occurred while processing your request. Please try again later.");
            return View(model);
        }
    }


    // GET : Account/EmployeeRegister
    [Authorize(Roles = "Admin")]
    public IActionResult EmployeeRegister()
    {
        return View();
    }

    // POST: Account/EmployeeRegister
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EmployeeRegister(EmployeeRegisterViewModel model)
    {

        if (ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            // Create a new user object and populate its properties
            var newEmployee = new PlatformUser
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = true
            };

            // Add the user to the database
            var returnedResult = await userManager.CreateAsync(newEmployee, model.Password);

            if (returnedResult.Succeeded)
            {
                // Add the Employee claim to the user
                await userManager.AddClaimAsync(newEmployee, new Claim(ClaimTypes.Role, "Employee"));

                // If the user is also a blogger, add the Blogger claim
                if (model.IsBlogger)
                {
                    await userManager.AddClaimAsync(newEmployee, new Claim(ClaimTypes.Role, "Blogger"));
                }

                // Add the required address
                var address = new Address
                {
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    Province = model.Province,
                    ZipCode = model.ZipCode
                };
                context.Addresses.Add(address);
                await context.SaveChangesAsync();

                var userAddress = new UserAddress
                {
                    PlatformUserId = newEmployee.Id,
                    AddressId = address.AddressId
                };
                
                context.UserAddresses.Add(userAddress);
                await context.SaveChangesAsync();

                // Add the optional address if provided
                if (!string.IsNullOrEmpty(model.OptionalAddressLine1) && !string.IsNullOrEmpty(model.OptionalCity) &&
                    !string.IsNullOrEmpty(model.OptionalProvince) && !string.IsNullOrEmpty(model.OptionalZipCode))
                {
                    var optionalAddress = new Address
                    {
                        AddressLine1 = model.OptionalAddressLine1,
                        AddressLine2 = model.OptionalAddressLine2,
                        City = model.OptionalCity,
                        Province = model.OptionalProvince,
                        ZipCode = model.OptionalZipCode
                    };
                    context.Addresses.Add(optionalAddress);
                    await context.SaveChangesAsync();

                    var optionalUserAddress = new UserAddress
                    {
                        PlatformUserId = newEmployee.Id,
                        AddressId = optionalAddress.AddressId
                    };
                    
                    context.UserAddresses.Add(optionalUserAddress);
                    await context.SaveChangesAsync();
                }

                // Redirect to a success page
                return RedirectToAction("EmployeeRegisterSuccess");
            }
            else
            {
                // If user creation fails, display errors
                foreach (var error in returnedResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }
        catch (Exception)
        {
            // Log the exception or handle it as needed
            ModelState.AddModelError(string.Empty,
                "An error occurred while processing your request. Please try again later.");
            return View();
        }
    }
    
    
    // GET: Account/AdminRegister
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult AdminRegister()
    {
        return View();
    }

    // POST: Account/AdminRegister
    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminRegister(FarmerRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var newAdmin = new PlatformUser
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newAdmin, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(newAdmin, new Claim(ClaimTypes.Role, "Admin"));
                await userManager.AddClaimAsync(newAdmin, new Claim(ClaimTypes.Role, "Employee"));
                await userManager.AddClaimAsync(newAdmin, new Claim(ClaimTypes.Role, "Blogger"));

                var address = new Address
                {
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    Province = model.Province,
                    ZipCode = model.ZipCode
                };
                
                context.Addresses.Add(address);
                await context.SaveChangesAsync();

                var userAddress = new UserAddress
                {
                    PlatformUserId = newAdmin.Id,
                    AddressId = address.AddressId
                };
                
                context.UserAddresses.Add(userAddress);
                await context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(model.OptionalAddressLine1) && !string.IsNullOrEmpty(model.OptionalCity) &&
                    !string.IsNullOrEmpty(model.OptionalProvince) && !string.IsNullOrEmpty(model.OptionalZipCode))
                {
                    var optionalAddress = new Address
                    {
                        AddressLine1 = model.OptionalAddressLine1,
                        AddressLine2 = model.OptionalAddressLine2,
                        City = model.OptionalCity,
                        Province = model.OptionalProvince,
                        ZipCode = model.OptionalZipCode
                    };
                    context.Addresses.Add(optionalAddress);
                    await context.SaveChangesAsync();

                    var optionalUserAddress = new UserAddress
                    {
                        PlatformUserId = newAdmin.Id,
                        AddressId = optionalAddress.AddressId
                    };
                    
                    context.UserAddresses.Add(optionalUserAddress);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("EmployeeRegisterSuccess");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
            return View(model);
        }
    }
    
    
    
    [HttpGet]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<IActionResult> AllFarmers(string? sortOrder, string? searchString)
    {
        var allUsers = await userManager.Users.ToListAsync();

        // Fetch claims for all users in a single query
        var allClaims = await context.UserClaims.ToListAsync();

        // Filter users who have the "Farmer" claim
        var farmerUsers = allUsers
            .Where(u => allClaims
                .Any(c => c.UserId == u.Id && c.ClaimType == ClaimTypes.Role && c.ClaimValue == "Farmer"));

        // Project to FarmerViewModel
        var farmers = farmerUsers.Select(u => new FarmerViewModel
        {
            Name = u.Name ?? string.Empty,
            Email = u.Email ?? string.Empty,
            ProductCount = context.Products.Count(p => p.PlatformUserId == u.Id),
            Id = u.Id
        });

        // Search functionality
        if (!String.IsNullOrEmpty(searchString))
        {
            farmers = farmers.Where(s => s.Name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase));
        }

        // Sorting
        ViewData["sortOrder"] = sortOrder;
        ViewData["CurrentFilter"] = searchString;

        switch (sortOrder)
        {
            case "name_asc":
                farmers = farmers.OrderBy(s => s.Name);
                break;
            case "name_desc":
                farmers = farmers.OrderByDescending(s => s.Name);
                break;
            case "product_asc":
                farmers = farmers.OrderBy(s => s.ProductCount);
                break;
            case "product_desc":
                farmers = farmers.OrderByDescending(s => s.ProductCount);
                break;
            default:
                farmers = farmers.OrderBy(s => s.Name);
                break;
        }

        return View(farmers.ToList());
    }




    
    
    // GET : Account/EmployeeRegisterSuccess
    public IActionResult EmployeeRegisterSuccess()
    {
        return View();
    }

    //GET : Account/AccessDenied 
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

}