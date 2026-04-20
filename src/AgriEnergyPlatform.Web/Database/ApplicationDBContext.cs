using AgriEnergyPlatform.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyPlatform.Web.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<PlatformUser>(options)
{
    public DbSet<Address> Addresses { get; init; }
    public DbSet<Blog> Blogs { get; init; }
    public DbSet<BlogCategory> BlogCategories { get; init; }
    public DbSet<CategoryBlog> CategoryBlogs { get; init; }
    public DbSet<CategoryProduct> CategoryProducts { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<ProductImage> ProductImages { get; init; }
    public DbSet<UserAddress> UserAddresses { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId);
    }
}