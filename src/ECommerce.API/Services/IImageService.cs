namespace ECommerce.API.Services;

/// <summary>
/// Interface for image upload service
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Upload image to cloud storage and return the URL
    /// </summary>
    /// <param name="file">Image file to upload</param>
    /// <returns>Public URL of uploaded image</returns>
    Task<string> UploadImageAsync(IFormFile file);
}
