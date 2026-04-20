namespace AgriEnergyPlatform.Web.Services.Storage;

public interface IImageStorage
{
    /// <param name="category">Path segment, e.g. "blogs" or "products".</param>
    /// <param name="fileNameWithoutExtension">Unique file name without extension; extension is taken from the upload.</param>
    Task<string> UploadAsync(IFormFile formFile, string category, string fileNameWithoutExtension);
}
