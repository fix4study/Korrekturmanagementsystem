using Azure.Storage.Blobs;
using Korrekturmanagementsystem.Services.Interfaces;
using Azure.Storage;
using Azure.Storage.Sas;


namespace Korrekturmanagementsystem.Services;

public class FileUploadService : IFileUploadService
{
    private readonly string _connectionString;
    private readonly string _containerName = "uploads";
    private readonly string _accountName;
    private readonly string _accountKey;


    public FileUploadService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureStorage:ConnectionString"]!;
        _accountName = configuration["AzureStorage:AccountName"]!;
        _accountKey = configuration["AzureStorage:AccountKey"]!;
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

    public string GenerateReadSasUrl(string fileName, TimeSpan validFor)
    {
        var credential = new StorageSharedKeyCredential(_accountName, _accountKey);
        var blobUri = new Uri($"https://{_accountName}.blob.core.windows.net/{_containerName}/{fileName}");
        var blobClient = new BlobClient(blobUri, credential);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerName,
            BlobName = fileName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
            ExpiresOn = DateTimeOffset.UtcNow.Add(validFor)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasUri = blobClient.GenerateSasUri(sasBuilder);
        return sasUri.ToString();
    }

}
