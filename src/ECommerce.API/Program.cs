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
// Priority: Environment Variable -> Configuration -> Development Default
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

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
    throw new InvalidOperationException("Database connection string not configured. Please set DATABASE_CONNECTION_STRING environment variable or DefaultConnection in configuration.");
}

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
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
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

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
    try
    {
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the database.");
    }
}

app.Run();
