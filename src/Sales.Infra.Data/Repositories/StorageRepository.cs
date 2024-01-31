using Google.Cloud.Storage.V1;
using Sales.Infra.Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Sales.Infra.Data.Repositories;

public class StorageRepository : IStorageRepository
{

    private const string defaultBucketName = "ocr-cnh";
    private const long extraTime = 3600000;

    private readonly StorageClient _client;

    private readonly UrlSigner _urlSigner;

    public StorageRepository(IConfiguration configuration)
    {
        _client = StorageClient.Create();
        _urlSigner = UrlSigner.FromCredentialFile("");
    }

    public async Task<string> UploadFileAsync(
        string filePath,
        string destination,
        string bucketName = defaultBucketName)
    {
        bucketName ??= defaultBucketName;

        var options = new UploadObjectOptions
        {
            PredefinedAcl = PredefinedObjectAcl.PublicRead,
            
        };

        var fileStream = File.OpenRead(filePath);
        {
            await _client.UploadObjectAsync(bucketName, destination, null, fileStream, options);

            var signedUrl = _urlSigner.Sign(
                bucketName,
                destination,
                TimeSpan.FromMilliseconds(extraTime),
                HttpMethod.Get
            );

            return signedUrl.ToString();
        }
    }
}
