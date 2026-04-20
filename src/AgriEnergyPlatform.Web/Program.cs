using AgriEnergyPlatform.Web.Database;
using AgriEnergyPlatform.Web.Models;
using AgriEnergyPlatform.Web.Services.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyPlatform.Web;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = ResolveConnectionString(builder);

        var storageProvider = builder.Configuration["Storage:Provider"] ?? "Local";
        if (string.Equals(storageProvider, "Firebase", StringComparison.OrdinalIgnoreCase))
            builder.Services.AddSingleton<IImageStorage, FirebaseImageStorage>();
        else
            builder.Services.AddSingleton<IImageStorage, LocalImageStorage>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(connectionString));
        builder.Services.AddIdentity<PlatformUser, IdentityRole>(
                options =>
                {
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireRole("Admin");
            });
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("EmployeeOnly", policy =>
            {
                policy.RequireRole("Employee");
            });
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ProductsView", policy =>
            {
                policy.RequireRole("Employee", "Farmer");
            });
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ProductsFunctions", policy =>
            {
                policy.RequireRole("Farmer", "Admin");
            });
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("FarmerOnly", policy =>
            {
                policy.RequireRole("Farmer");
            });
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        var app = builder.Build();

        if (app.Configuration.GetValue<bool>("Database:AutoMigrate"))
        {
            EnsureSqlServerDatabaseExists(connectionString);
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        if (!app.Configuration.GetValue<bool>("DisableHttpsRedirect"))
            app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "productDetails",
            pattern: "Product/Details/{id?}",
            defaults: new { controller = "Product", action = "Details" });

        app.MapControllerRoute(
            name: "slugRoute",
            pattern: "{slug}",
            defaults: new { controller = "Blog", action = "GetBlog" });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static string ResolveConnectionString(WebApplicationBuilder builder)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        if (!string.IsNullOrEmpty(dbHost))
        {
            var dbName = Environment.GetEnvironmentVariable("DB_NAME")
                ?? throw new InvalidOperationException("DB_NAME is required when DB_HOST is set.");
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD")
                ?? throw new InvalidOperationException("DB_SA_PASSWORD is required when DB_HOST is set.");
            return $"Data Source={dbHost},1433;Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True;Encrypt=False";
        }

        var fromConfig = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? builder.Configuration.GetConnectionString("default");
        if (string.IsNullOrEmpty(fromConfig))
            throw new InvalidOperationException(
                "Configure a database: set DB_HOST, DB_NAME, DB_SA_PASSWORD for Docker, or ConnectionStrings:DefaultConnection in appsettings.");

        return fromConfig;
    }

    private static void EnsureSqlServerDatabaseExists(string connectionString)
    {
        var csb = new SqlConnectionStringBuilder(connectionString);
        var dbName = csb.InitialCatalog;
        if (string.IsNullOrEmpty(dbName))
            return;

        csb.InitialCatalog = "master";
        using var conn = new SqlConnection(csb.ConnectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        var safeName = dbName.Replace("]", "]]");
        cmd.CommandText = $"""
            IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{dbName.Replace("'", "''")}')
            CREATE DATABASE [{safeName}]
            """;
        cmd.ExecuteNonQuery();
    }
}
