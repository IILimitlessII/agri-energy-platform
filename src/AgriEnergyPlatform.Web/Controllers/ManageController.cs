using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AgriEnergyPlatform.Web.Database;
using AgriEnergyPlatform.Web.Models;
using AgriEnergyPlatform.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyPlatform.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<PlatformUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ManageController(UserManager<PlatformUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /Manage
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userAddresses = await _context.UserAddresses
                .Include(ua => ua.Address)
                .Where(ua => ua.PlatformUserId == userId)
                .ToListAsync();

            var productsCount = await _context.Products
                .Where(p => p.PlatformUserId == userId)
                .CountAsync();

            var blogsCount = await _context.Blogs
                .Where(b => b.PlatformUserId == userId)
                .CountAsync();

            var model = new ManageViewModel
            {
                User = user,
                UserAddresses = userAddresses,
                ProductsCount = productsCount,
                BlogsCount = blogsCount
            };

            return View(model);
        }
        
        // POST: /Manage/UpdateUser
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            return Ok();
        }

        // POST: /Manage/UpdateAddress
        [HttpPost]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userAddress = await _context.UserAddresses
                .Include(ua => ua.Address)
                .FirstOrDefaultAsync(ua => ua.PlatformUserId == userId && ua.AddressId == model.AddressId);

            if (userAddress == null)
            {
                return NotFound();
            }

            // Update address details
            if (userAddress.Address != null)
            {
                userAddress.Address.AddressLine1 = model.AddressLine1;
                userAddress.Address.AddressLine2 = model.AddressLine2;
                userAddress.Address.City = model.City;
                userAddress.Address.Province = model.Province;
                userAddress.Address.ZipCode = model.ZipCode;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: /Manage/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        
        // GET: /Manage/Admins
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ViewAdmin()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var model = new AdminViewModel
            {
                Admins = admins.ToList()
            };
            return View(model);
        }
        

        // POST: /Manage/DeleteProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(Index), new { ErrorMessage = "Failed to delete profile" });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
