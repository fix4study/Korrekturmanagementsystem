namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadAsync(string fileName, Stream content);
    string GenerateReadSasUrl(string fileName, TimeSpan validFor);
}
