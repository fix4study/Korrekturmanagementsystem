using Korrekturmanagementsystem.Shared;

using Microsoft.AspNetCore.Components.Forms;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IFileUploadService
{
    Task<Result> UploadAsync(Guid reportId, List<IBrowserFile> files);
    string GenerateReadSasUrl(string fileName, TimeSpan validFor);
}
