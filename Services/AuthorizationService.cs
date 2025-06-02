using Korrekturmanagementsystem.Models.Enums;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IReportService _reportService;

    public AuthorizationService(
        ICurrentUserService currentUserService,
        IReportService reportService)
    {
        _currentUserService = currentUserService;
        _reportService = reportService;
    }

    public async Task<bool> HasEditReportPermissonAsync(Guid reportId)
    {
        var userId = _currentUserService.GetCurrentUserId();
        if (userId is null)
        {
            return false;
        }

        var creatorUserId = await _reportService.GetCreatorUserIdByReportIdAsync(reportId);
        if (creatorUserId is null)
        {
            return false;
        }

        if (creatorUserId == userId)
        {
            return true;
        }

        var userRole = _currentUserService.GetCurrentUserRole();

        if (Enum.TryParse<SystemRole>(userRole, out var systemRole))
        {
            return systemRole is SystemRole.Intern || systemRole is SystemRole.Admin;
        }

        return false;
    }
}
