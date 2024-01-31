

namespace Sales.Infra.Data.Interfaces;

public interface IStorageRepository
{
    Task<string> UploadFileAsync(string filePath, string destination, string bucketName);
}
