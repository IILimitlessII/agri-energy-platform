using Firebase.Auth;
using Firebase.Storage;

namespace AgriEnergyPlatform.Web.Services.Storage;

public class FirebaseImageStorage : IImageStorage
{
    private readonly string _apiKey;
    private readonly string _bucket;

    public FirebaseImageStorage(IConfiguration configuration)
    {
        _apiKey = configuration["Storage:Firebase:ApiKey"]
            ?? throw new InvalidOperationException("Storage:Firebase:ApiKey is required when Storage:Provider is Firebase.");
        _bucket = configuration["Storage:Firebase:Bucket"]
            ?? throw new InvalidOperationException("Storage:Firebase:Bucket is required when Storage:Provider is Firebase.");
    }

    public async Task<string> UploadAsync(IFormFile formFile, string category, string fileNameWithoutExtension)
    {
        var ext = Path.GetExtension(formFile.FileName);
        if (string.IsNullOrEmpty(ext))
            ext = ".jpg";

        var imageName = $"{fileNameWithoutExtension}{ext}";

        var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
        var authResult = await auth.SignInAnonymouslyAsync();

        await using var stream = formFile.OpenReadStream();

        var firebaseStorage = new FirebaseStorage(
            _bucket,
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authResult.FirebaseToken)
            });

        var uploadTask = firebaseStorage
            .Child(category)
            .Child(imageName)
            .PutAsync(stream);

        return await uploadTask;
    }
}
