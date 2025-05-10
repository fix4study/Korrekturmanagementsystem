using Azure.Storage.Blobs;
using Korrekturmanagementsystem.Services.Interfaces;
using Azure.Storage;
using Azure.Storage.Sas;
using Korrekturmanagementsystem.Dtos;
using Microsoft.AspNetCore.Components.Forms;


namespace Korrekturmanagementsystem.Services;

public class FileUploadService : IFileUploadService
{
    private readonly string _connectionString;
    private readonly string _containerName = "uploads";
    private readonly string _accountName;
    private readonly string _accountKey;
    private readonly IAttachmentService _attachmentService;


    public FileUploadService(IConfiguration configuration, IAttachmentService attachmentService)
    {
        _connectionString = configuration["AzureStorage:ConnectionString"]!;
        _accountName = configuration["AzureStorage:AccountName"]!;
        _accountKey = configuration["AzureStorage:AccountKey"]!;
        _attachmentService = attachmentService;
    }

    public async Task<bool> UploadAsync(Guid reportId, List<IBrowserFile> files)
    {
        try
        {
            foreach (var file in files)
            {

                using var stream = file.OpenReadStream();
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(file.Name);
                await blobClient.UploadAsync(stream, overwrite: true);

                var blobUrl = blobClient.Uri.ToString();

                var attachment = new CreateAttachmentDto
                {
                    ReportId = reportId,
                    FileName = file.Name,
                    FileUrl = blobUrl,
                    UploadedAt = DateTime.UtcNow
                };

                await _attachmentService.CreateAsync(attachment);
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

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
