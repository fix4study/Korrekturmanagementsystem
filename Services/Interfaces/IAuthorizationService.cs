namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IAuthorizationService
{
    Task<bool> HasEditReportPermissonAsync(Guid reportId);
}
