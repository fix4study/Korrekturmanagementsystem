using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportProvider
{
    Task<Guid?> AddReportAsync(AddReportDto report, Guid userId);
    Task<IEnumerable<ReportOverviewDto>> GetReportsOverviewAsync();
    Task<ReportFormOptionsDto> GetFormOptionsAsync();
    Task<ReportDetailsDto> GetReportDetailsByIdAsync(Guid id);
    Task<Result> UpdateReportByIdAsync(ReportDto reportToUpdate);
    Task<IEnumerable<ReportOverviewDto>> GetAllReportByUserIdAsync(Guid userId);
}
