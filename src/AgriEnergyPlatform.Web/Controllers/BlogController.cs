using System;
using System.Globalization;
using System.Security.Claims;
using AgriEnergyPlatform.Web.Database;
using AgriEnergyPlatform.Web.Models;
using AgriEnergyPlatform.Web.Services.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyPlatform.Web.Controllers;

public class BlogController : Controller
{
    private readonly SignInManager<PlatformUser> _signInManager;
    private readonly UserManager<PlatformUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IImageStorage _imageStorage;

    public BlogController(
            SignInManager<PlatformUser> signInManager,
            UserManager<PlatformUser> userManager,
            ApplicationDbContext context,
            IImageStorage imageStorage)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _imageStorage = imageStorage;
    }

    // GET: Category/Index
    public IActionResult Index()
    {
        var blogs = _context.Blogs.ToList();
        return View(blogs);
    }

    // GET: CategoryIndex
    public IActionResult CategoryIndex()
    {
        var categories = _context.CategoryBlogs.ToList();
        return View(categories);
    }

    // GET
    public IActionResult CreateCategory()
    {
        return View();
    }

    // POST
    [HttpPost]
    public IActionResult CreateCategory(CategoryBlog category)
    {
        if (ModelState.IsValid)
        {
            _context.CategoryBlogs.Add(category);
            _context.SaveChanges();
            return RedirectToAction("CategoryIndex");
        }

        return View(category);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBlog(string slug)
    {
        var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Slug == slug);
        if (blog == null)
        {
            return NotFound();
        }

        // Retrieve the user associated with the blog
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == blog.PlatformUserId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        // Pass the username to ViewData
        ViewData["UserName"] = user.UserName;

        return View(blog);
    }





    // GET
    public IActionResult CreateBlog()
    {
        // Retrieve categories from the database
        var categories = _context.CategoryBlogs.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        // Add an option for "Select Category" if needed
        categories.Insert(0, new SelectListItem { Value = "", Text = "Select Category" });

        // Pass categories to the view
        ViewBag.Categories = categories;

        return View();
    }


    
    [HttpPost]
    public async Task<IActionResult> CreateBlog([FromForm] BlogCreateDto blogCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Upload images and get URLs
        var blogImageUrl = await _imageStorage.UploadAsync(blogCreateDto.BlogImage, "blogs", $"{Guid.NewGuid()}_blogImage");
        var heading1ImageUrl = blogCreateDto.Heading1Image != null
            ? await _imageStorage.UploadAsync(blogCreateDto.Heading1Image, "blogs", $"{Guid.NewGuid()}_heading1Image")
            : null;
        var heading2ImageUrl = blogCreateDto.Heading2Image != null
            ? await _imageStorage.UploadAsync(blogCreateDto.Heading2Image, "blogs", $"{Guid.NewGuid()}_heading2Image")
            : null;
        var heading3ImageUrl = blogCreateDto.Heading3Image != null
            ? await _imageStorage.UploadAsync(blogCreateDto.Heading3Image, "blogs", $"{Guid.NewGuid()}_heading3Image")
            : null;
        var heading4ImageUrl = blogCreateDto.Heading4Image != null
            ? await _imageStorage.UploadAsync(blogCreateDto.Heading4Image, "blogs", $"{Guid.NewGuid()}_heading4Image")
            : null;
        var heading5ImageUrl = blogCreateDto.Heading5Image != null
            ? await _imageStorage.UploadAsync(blogCreateDto.Heading5Image, "blogs", $"{Guid.NewGuid()}_heading5Image")
            : null;

        string date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        
        // Parse the date string into a DateTime object
        DateTime parsedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        // Extract the date part from the DateTime object and convert it to DateOnly
        DateOnly publishDate = new DateOnly(parsedDate.Year, parsedDate.Month, parsedDate.Day);


        // Create a slug from the title (assuming a simple slug creation)
        string slug = blogCreateDto.Title.ToLower().Replace(" ", "-");

        // Get the currently logged-in user
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var blog = new Blog
                {
                    Title = blogCreateDto.Title,
                    BlogImageUrl = blogImageUrl,
                    Subtitle = blogCreateDto.Subtitle,
                    Intro = blogCreateDto.Intro,
                    Heading1 = blogCreateDto.Heading1,
                    Content1 = blogCreateDto.Content1,
                    Heading1ImageUrl = heading1ImageUrl,
                    Heading2 = blogCreateDto.Heading2,
                    Content2 = blogCreateDto.Content2,
                    Heading2ImageUrl = heading2ImageUrl,
                    Heading3 = blogCreateDto.Heading3,
                    Content3 = blogCreateDto.Content3,
                    Heading3ImageUrl = heading3ImageUrl,
                    Heading4 = blogCreateDto.Heading4,
                    Content4 = blogCreateDto.Content4,
                    Heading4ImageUrl = heading4ImageUrl,
                    Heading5 = blogCreateDto.Heading5,
                    Content5 = blogCreateDto.Content5,
                    Heading5ImageUrl = heading5ImageUrl,
                    PublishDate = publishDate,
                    Slug = slug,
                    PlatformUserId = currentUserId
                };

                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync();

                // Add the category to the blog
                var categoryBlog = new BlogCategory
                {
                    BlogId = blog.Id,
                    CategoryBlogId = blogCreateDto.CategoryId
                };

                _context.BlogCategories.Add(categoryBlog);
                await _context.SaveChangesAsync();

                transaction.Commit(); // Commit the transaction
                return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
            }
            catch (Exception)
            {
                transaction.Rollback(); // Rollback the transaction in case of an error
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}