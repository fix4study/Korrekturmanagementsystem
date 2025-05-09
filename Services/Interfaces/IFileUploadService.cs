namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadAsync(string fileName, Stream content);
}
