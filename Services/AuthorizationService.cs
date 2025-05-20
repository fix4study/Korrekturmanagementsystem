using Korrekturmanagementsystem.Models.Enums;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IReportProvider _reportProvider;
    private readonly IUserProvider _userProvider;

    public AuthorizationService(
        ICurrentUserService currentUserService,
        IReportProvider reportProvider,
        IUserProvider userProvider)
    {
        _currentUserService = currentUserService;
        _reportProvider = reportProvider;
        _userProvider = userProvider;
    }

    public async Task<bool> HasEditReportPermissonAsync(Guid reportId)
    {
        var userId = _currentUserService.GetCurrentUserId();
        if (userId is null)
        {
            return false;
        }

        var creatorUserId = await _reportProvider.GetCreatorUserIdByReportIdAsync(reportId);
        if (creatorUserId == null)
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
