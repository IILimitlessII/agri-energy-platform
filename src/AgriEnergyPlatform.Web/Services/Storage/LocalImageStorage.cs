namespace AgriEnergyPlatform.Web.Services.Storage;

public class LocalImageStorage : IImageStorage
{
    private readonly IWebHostEnvironment _env;

    public LocalImageStorage(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadAsync(IFormFile formFile, string category, string fileNameWithoutExtension)
    {
        var ext = Path.GetExtension(formFile.FileName);
        if (string.IsNullOrEmpty(ext))
            ext = ".jpg";

        var safeCategory = SanitizeSegment(category);
        var fileName = $"{SanitizeSegment(fileNameWithoutExtension)}{ext}";
        var relativeDir = Path.Combine("uploads", safeCategory);
        var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
        var dir = Path.Combine(webRoot, relativeDir);
        Directory.CreateDirectory(dir);

        var physicalPath = Path.Combine(dir, fileName);
        await using (var stream = new FileStream(physicalPath, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }

        var urlPath = $"{relativeDir.Replace('\\', '/')}/{fileName}";
        return "/" + urlPath.TrimStart('/');
    }

    private static string SanitizeSegment(string segment)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            segment = segment.Replace(c, '_');
        return string.IsNullOrWhiteSpace(segment) ? "misc" : segment;
    }
}
