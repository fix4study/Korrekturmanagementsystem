using Azure.Storage.Blobs;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class FileUploadService : IFileUploadService
{
    private readonly string _connectionString;
    private readonly string _containerName = "uploads";

    public FileUploadService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureStorage:ConnectionString"]!;
    }

    public async Task<string> UploadAsync(string fileName, Stream content)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(content, overwrite: true);

        return blobClient.Uri.ToString();
    }
}
