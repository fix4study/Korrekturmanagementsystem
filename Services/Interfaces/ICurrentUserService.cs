namespace Korrekturmanagementsystem.Services.Interfaces;

public interface ICurrentUserService
{
    Guid? GetCurrentUserId();
    string? GetCurrentUserName();
    string? GetCurrentUserRole();
    bool IsAuthenticated { get; }
}
