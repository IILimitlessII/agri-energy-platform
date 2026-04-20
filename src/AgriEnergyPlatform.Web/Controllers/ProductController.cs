using System.Globalization;
using AgriEnergyPlatform.Web.Database;
using AgriEnergyPlatform.Web.Models;
using AgriEnergyPlatform.Web.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AgriEnergyPlatform.Web.ViewModels;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging;

namespace AgriEnergyPlatform.Web.Controllers
{
    public class ProductController(ApplicationDbContext context, IImageStorage imageStorage) : Controller
    {

        [HttpGet]
        [Authorize(Policy = "ProductsView")]
        public IActionResult ViewProducts(string? userId, string? searchString, int? categoryId,
    DateOnly? startDate, DateOnly? endDate, int pageNumber = 1)
        {
            const int PageSize = 15;

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            List<Product> products;

            var categories = context.CategoryProducts.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            if (userRoles.Contains("Employee"))
            {
                products = !string.IsNullOrEmpty(userId)
                    ? context.Products.Include(p => p.CategoryProduct).Where(p => p.PlatformUserId == userId).ToList()
                    : context.Products.Include(p => p.CategoryProduct).ToList();
            }
            else if (userRoles.Contains("Farmer"))
            {
                products = context.Products.Include(p => p.CategoryProduct)
                    .Where(p => p.PlatformUserId == currentUserId).ToList();
            }
            else
            {
                products = new List<Product>();
            }

            if (categoryId.HasValue && categoryId != 0)
            {
                products = products.Where(p => p.CategoryProduct != null && p.CategoryProduct.Id == categoryId).ToList();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products
                    .Where(p => !string.IsNullOrEmpty(p.Name) &&
                                p.Name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate >= startDate && p.ProductionDate <= endDate).ToList();
            }

            var totalProducts = products.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);
            var productsOnPage = products.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            var viewModel = new ProductFilterViewModel
            {
                Products = productsOnPage,
                SelectedCategoryId = categoryId ?? 0,
                Categories = categories,
                StartDate = startDate,
                EndDate = endDate,
                SearchString = searchString ?? string.Empty
            };

            ViewData["Context"] = context;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            return View(viewModel);
        }




        // GET : Product/Details/5
        [HttpGet]
        [Authorize(Policy = "ProductsView")]
        public IActionResult Details(int id)
        {
            // Get product Details using its id
            var product = context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Get Other products that are made by the same farmer
            var relatedProducts = context.Products
                .Where(p => p.PlatformUserId == product.PlatformUserId && p.Id != id)
                .ToList();

            ViewData["Context"] = context;
            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }


        [HttpGet]
        [Authorize(Policy = "FarmerOnly")]
        public async Task<IActionResult> Create()
        {
            // Get the list of categories from the database
            var categories = await context.CategoryProducts.ToListAsync();

            // Pass the categories to the view
            ViewData["Categories"] = categories;

            // Create an instance of the ProductViewModel
            var model = new ProductViewModel();

            // Return the view with the populated model
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model, bool createCategory)
        {
            // Check if errors are thrown or fields missing
            if (ModelState.IsValid)
            {
                // Parse the Price value as a decimal
                if (!decimal.TryParse(model.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                {
                    ModelState.AddModelError(nameof(model.Price), "Invalid price format.");
                    // Repopulate Categories for dropdown
                    ViewData["Categories"] = await context.CategoryProducts.ToListAsync();
                    return View(model);
                }

                // Start Transaction don't want to save to database if images don't upload properly or error is thrown
                await using var transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    // Get highest productId
                    int highestProductId = GetHighestProductId() + 1;

                    // Upload images first
                    List<ProductImage> productImages = await UploadImages(imageStorage, model.Images, highestProductId);
                    if (createCategory)
                    {
                        CategoryProduct? newCategory = await CreateNewCategory(model.NewCategoryName);
                        if (newCategory != null)
                        {
                            model.CategoryProductId = newCategory.Id;
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Failed to create a new category.");
                            // Repopulate Categories for dropdown
                            ViewData["Categories"] = await context.CategoryProducts.ToListAsync();
                            return View(model);
                        }
                    }
                    else
                    {
                        // Repopulate Categories for dropdown
                        ViewData["Categories"] = await context.CategoryProducts.ToListAsync();
                    }

                    if (productImages.Any())
                    {
                        // Create the new product but don't save it yet
                        // Associate product with images
                        var product = new Product
                        {
                            Name = model.Name,
                            Description = model.Description,
                            ProductionDate = DateOnly.FromDateTime(model.ProductionDate),
                            Quantity = model.Quantity,
                            Price = price,
                            PlatformUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                            CategoryProductId = model.CategoryProductId,
                            ProductImages = productImages,
                            IsWeight = model.IsWeight
                        };

                        context.Products.Add(product);
                        await context.SaveChangesAsync();

                        // Set the ProductImageId of the product to the first image uploaded
                        product.ProductImageId = productImages.First().Id;
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return RedirectToAction(nameof(ViewProducts));
                    }
                }
                catch (Exception)
                {
                    // Rollback the commit because an error occurred
                    await transaction.RollbackAsync();
                    ModelState.AddModelError(string.Empty,
                        "An error occurred while creating the product. Please try again.");
                }
            }

            return View(model);
        }


        private async Task<CategoryProduct?> CreateNewCategory(string? categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return null;
            }

            // Check if a category with the same name already exists
            var existingCategory = await context.CategoryProducts.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (existingCategory != null)
            {
                // If a category with the same name already exists, return it
                return existingCategory;
            }
            else
            {
                // If the category doesn't exist, create a new one
                var newCategory = new CategoryProduct
                {
                    Name = categoryName
                };

                context.CategoryProducts.Add(newCategory);
                await context.SaveChangesAsync();

                return newCategory;
            }
        }


        // GET: Products/Edit/5
        [HttpGet]
        [Authorize(Policy = "ProductsFunctions")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var categories = await context.CategoryProducts.ToListAsync();
            ViewData["Categories"] = categories;

            var model = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name ?? string.Empty,
                Description = product.Description ?? string.Empty,
                ProductionDate = product.ProductionDate.ToDateTime(TimeOnly.MinValue),
                Quantity = product.Quantity,
                IsWeight = product.IsWeight,
                Price = product.Price.ToString(CultureInfo.InvariantCulture),
                CategoryProductId = product.CategoryProductId ?? 0
            };

            return View(model);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model, bool createCategory)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!decimal.TryParse(model.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                {
                    ModelState.AddModelError(nameof(model.Price), "Invalid price format.");
                    ViewData["Categories"] = await context.CategoryProducts.ToListAsync();
                    return View(model);
                }

                try
                {
                    var product = await context.Products
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.ProductionDate = DateOnly.FromDateTime(model.ProductionDate);
                    product.Quantity = model.Quantity;
                    product.IsWeight = model.IsWeight;
                    product.Price = price;

                    if (createCategory)
                    {
                        var newCategory = await CreateNewCategory(model.NewCategoryName);
                        if (newCategory != null)
                        {
                            product.CategoryProductId = newCategory.Id;
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Failed to create a new category.");
                            ViewData["Categories"] = await context.CategoryProducts.ToListAsync();
                            return View(model);
                        }
                    }
                    else
                    {
                        product.CategoryProductId = model.CategoryProductId;
                    }

                    context.Update(product);
                    await context.SaveChangesAsync();

                    return RedirectToAction(nameof(ViewProducts));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["Categories"] = await context.CategoryProducts.ToListAsync();
            return View(model);
        }
        
        // DELETE    : Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ProductsFunctions")]
        public async Task<IActionResult> Delete(int id)
        {
            // Get userId
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            // Get product using id
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Check if the same user that wants to delete this product, made the product listing or is an Admin
            if (product.PlatformUserId == userId || User.HasClaim(c => c is { Type: ClaimTypes.Role, Value: "Admin" }))
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }

            // Return JSON
            return RedirectToAction("ViewProducts");
        }

        public int GetHighestProductId()
        {
            // Query the database to retrieve the highest ProductId
            var highestProductId = context.Products
                .OrderByDescending(p => p.Id)
                .Select(p => p.Id)
                .FirstOrDefault();

            return highestProductId;
        }

        private static async Task<List<ProductImage>> UploadImages(
            IImageStorage imageStorage,
            List<IFormFile> images,
            int productId)
        {
            var productImages = new List<ProductImage>();
            int i = 1;
            foreach (var formFile in images)
            {
                if (formFile.Length > 0)
                {
                    string imageName = $"{productId}_{i}";
                    string imageUrl = await imageStorage.UploadAsync(formFile, "products", imageName);

                    var productImage = new ProductImage
                    {
                        Url = imageUrl
                    };
                    productImages.Add(productImage);
                    i++;
                }
            }

            return productImages;
        }

        private bool ProductExists(int id)
        {
            return context.Products.Any(e => e.Id == id);
        }
    }
}