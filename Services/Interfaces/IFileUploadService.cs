using Microsoft.AspNetCore.Components.Forms;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IFileUploadService
{
    Task<bool> UploadAsync(Guid reportId, List<IBrowserFile> files);
    string GenerateReadSasUrl(string fileName, TimeSpan validFor);
}
