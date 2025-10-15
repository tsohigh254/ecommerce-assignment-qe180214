using System.Text;
using System.Text.Json;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AccountController> _logger;
    private readonly string _apiBaseUrl;

    public AccountController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<AccountController> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        // Priority: Environment Variable -> Configuration -> Development Default
        _apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL")
            ?? configuration["ApiSettings:BaseUrl"] 
            ?? "http://api:8080";
        _logger.LogInformation($"API Base URL configured as: {_apiBaseUrl}");
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        // Redirect to home if already logged in
        if (HttpContext.Session.GetString("JWTToken") != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var registerDto = new
            {
                email = model.Email,
                password = model.Password,
                confirmPassword = model.ConfirmPassword,
                firstName = model.FirstName,
                lastName = model.LastName
            };

            var json = JsonSerializer.Serialize(registerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
                {
                    // Store JWT token in session
                    HttpContext.Session.SetString("JWTToken", authResponse.Token);
                    HttpContext.Session.SetString("UserEmail", authResponse.Email);
                    HttpContext.Session.SetString("UserName", $"{authResponse.FirstName} {authResponse.LastName}");

                    TempData["SuccessMessage"] = "Registration successful! Welcome to our store.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Registration failed: {errorContent}");
                
                // Try to parse error message
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    ModelState.AddModelError(string.Empty, errorResponse?.Message ?? "Registration failed. Please try again.");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
        }

        return View(model);
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        // Redirect to home if already logged in
        if (HttpContext.Session.GetString("JWTToken") != null)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var loginDto = new
            {
                email = model.Email,
                password = model.Password
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
                {
                    // Store JWT token in session
                    HttpContext.Session.SetString("JWTToken", authResponse.Token);
                    HttpContext.Session.SetString("UserEmail", authResponse.Email);
                    HttpContext.Session.SetString("UserName", $"{authResponse.FirstName} {authResponse.LastName}");

                    TempData["SuccessMessage"] = "Login successful!";

                    // Redirect to return URL or home
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Login failed: {errorContent}");
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
        }

        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        // Clear session
        HttpContext.Session.Remove("JWTToken");
        HttpContext.Session.Remove("UserEmail");
        HttpContext.Session.Remove("UserName");
        HttpContext.Session.Clear();

        TempData["SuccessMessage"] = "You have been logged out successfully.";
        return RedirectToAction("Index", "Home");
    }

    // Helper classes for JSON deserialization
    private class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}
