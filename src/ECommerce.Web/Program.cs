using ECommerce.Web.Services;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// DATA PROTECTION CONFIGURATION
// ========================================
// Khắc phục lỗi: "The key {...} was not found in the key ring"
// Data Protection được dùng để mã hóa Session, Antiforgery tokens, Authentication cookies
var dataProtectionBuilder = builder.Services.AddDataProtection()
    .SetApplicationName("ECommerce.Web"); // Đảm bảo cùng tên với API nếu cần share keys

// Check for Redis connection string (recommended for production/load balancing)
var redisConnection = builder.Configuration.GetConnectionString("Redis")
    ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

if (!string.IsNullOrEmpty(redisConnection))
{
    try
    {
        // Use Redis for persistent key storage (recommended for production)
        var redis = StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnection);
        dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        Console.WriteLine("Data Protection: Using Redis for key storage");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"WARNING: Failed to connect to Redis: {ex.Message}. Falling back to file system.");
        // Fallback to file system if Redis fails
        var keyPath = Path.Combine(Directory.GetCurrentDirectory(), "keys");
        Directory.CreateDirectory(keyPath);
        dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(keyPath));
    }
}
else
{
    // Fallback: Use file system for key storage (works for single instance)
    var keyPath = Path.Combine(Directory.GetCurrentDirectory(), "keys");
    Directory.CreateDirectory(keyPath);
    dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(keyPath));
    Console.WriteLine($"Data Protection: Using file system for key storage at {keyPath}");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Bắt buộc HTTPS trong production
    options.Cookie.SameSite = SameSiteMode.Lax; // Bảo vệ CSRF
});

// Add HttpClient for API calls
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Register services
builder.Services.AddHttpClient<ProductService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHttpClient<IPaymentService, PaymentService>(client =>
{
    // Priority: Environment Variable -> Configuration -> Development Default
    var apiUrl = Environment.GetEnvironmentVariable("API_BASE_URL")
        ?? builder.Configuration["ApiSettings:BaseUrl"] 
        ?? "http://api:8080";
    client.BaseAddress = new Uri(apiUrl);
});

var app = builder.Build();

// Configure URLs for production
if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Urls.Add($"http://0.0.0.0:{port}");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Only use HTTPS redirection in production
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

// Add Session middleware (must be before UseAuthorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
