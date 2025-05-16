namespace Korrekturmanagementsystem.Services.Interfaces;

public interface ICurrentUserService
{
    Guid GetCurrentUserId();
    string? GetCurrentUserName();
    bool IsAuthenticated { get; }
}
