using Korrekturmanagementsystem.Services.Interfaces;
using System.Security.Claims;

namespace Korrekturmanagementsystem.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentUserId()
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out var userId))
        {
            return null;
        }

        return userId;
    }

    public string? GetCurrentUserName()
        => _httpContextAccessor.HttpContext?.User.Identity?.Name;

    public string? GetCurrentUserRole()
        => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
