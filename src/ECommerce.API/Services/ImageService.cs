using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommerce.API.Configuration;
using Microsoft.Extensions.Options;

namespace ECommerce.API.Services;

/// <summary>
/// Service for uploading images to Cloudinary
/// </summary>
public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<ImageService> _logger;
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public ImageService(IOptions<CloudinarySettings> config, ILogger<ImageService> logger)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        if (logger == null) throw new ArgumentNullException(nameof(logger));

        _logger = logger;
        
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        try
        {
            // Validate file
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null");
            }

            // Check file size
            if (file.Length > _maxFileSize)
            {
                throw new ArgumentException($"File size exceeds maximum allowed size of {_maxFileSize / (1024 * 1024)}MB");
            }

            // Check file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                throw new ArgumentException($"File type {extension} is not allowed. Allowed types: {string.Join(", ", _allowedExtensions)}");
            }

            // Upload to Cloudinary
            using var stream = file.OpenReadStream();
            
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "ecommerce-products", // Organize in a folder
                Transformation = new Transformation()
                    .Width(800)
                    .Height(800)
                    .Crop("limit") // Limit size while maintaining aspect ratio
                    .Quality("auto") // Auto quality optimization
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                _logger.LogError($"Cloudinary upload error: {uploadResult.Error.Message}");
                throw new Exception($"Image upload failed: {uploadResult.Error.Message}");
            }

            _logger.LogInformation($"Image uploaded successfully: {uploadResult.SecureUrl}");
            return uploadResult.SecureUrl.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image to Cloudinary");
            throw;
        }
    }
}
