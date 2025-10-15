using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ECommerce.API.Data;
using ECommerce.API.Configuration;
using ECommerce.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework
// Priority: ConnectionStrings__DefaultConnection (Render format) -> DATABASE_CONNECTION_STRING -> appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

// Fallback for development only - use a clearly marked placeholder and require developers to set .env or environment variable
if (string.IsNullOrEmpty(connectionString) && builder.Environment.IsDevelopment())
{
    // Important: do NOT check in real credentials. Developers should create a local `.env` file
    // or set the DATABASE_CONNECTION_STRING environment variable. This placeholder is intentionally
    // non-secret and will not work for production.
    connectionString = "Host=localhost;Database=ecommerce;Username=postgres;Password=changeme_local";
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string not configured. Please set ConnectionStrings__DefaultConnection or DATABASE_CONNECTION_STRING environment variable.");
}

Console.WriteLine($"Using connection string: {new string('*', Math.Min(connectionString.Length, 20))}... (masked for security)");

builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure JWT Settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);
var jwtConfig = jwtSettings.Get<JwtSettings>();

// Configure Cloudinary Settings
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

// Configure Stripe Settings
builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("StripeSettings"));

// Register Image Upload Service
builder.Services.AddScoped<ECommerce.API.Services.IImageService, ECommerce.API.Services.ImageService>();

// Register Payment Service
builder.Services.AddScoped<ECommerce.API.Services.IPaymentService, ECommerce.API.Services.StripePaymentService>();

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ECommerceDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig?.Issuer,
        ValidAudience = jwtConfig?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtConfig?.SecretKey ?? "DefaultSecretKey"))
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        if (builder.Environment.IsProduction())
        {
            // Production: Use specific allowed origins from configuration
            var allowedOrigins = builder.Configuration
                .GetSection("AllowedOrigins")
                .Get<string[]>();
            
            if (allowedOrigins != null && allowedOrigins.Length > 0)
            {
                corsBuilder.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
            }
            else
            {
                // Fallback: Allow any origin if not configured (not recommended)
                Console.WriteLine("WARNING: AllowedOrigins not configured for production!");
                corsBuilder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
            }
        }
        else
        {
            // Development: Allow any origin for easier testing
            corsBuilder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
        }
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure URLs for production
if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Urls.Add($"http://0.0.0.0:{port}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowAll");

// Only use HTTPS redirection in production
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Run database migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var context = services.GetRequiredService<ECommerceDbContext>();
        
        logger.LogInformation("Starting database migration...");
        
        // Apply any pending migrations
        context.Database.Migrate();
        
        logger.LogInformation("Database migration completed successfully");
        
        // Check if we have any products
        var productCount = await context.Products.CountAsync();
        logger.LogInformation($"Database contains {productCount} products");
        
        if (productCount == 0)
        {
            logger.LogWarning("No products found in database. Seed data may not have been applied.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        // Log connection string (masked) for debugging
        var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connStr))
        {
            var maskedConnStr = connStr.Length > 30 
                ? connStr.Substring(0, 20) + "..." + connStr.Substring(connStr.Length - 10)
                : new string('*', connStr.Length);
            logger.LogError($"Connection string format: {maskedConnStr}");
        }
        throw; // Re-throw to prevent startup with broken database
    }
}

app.Run();
