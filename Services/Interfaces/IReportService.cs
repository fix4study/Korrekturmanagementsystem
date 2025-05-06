using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportService
{
    Task<bool> AddReportAsync(AddReportDto report);
    Task<IEnumerable<ReportOverviewDto>> GetReportsOverviewAsync();
    Task<ReportFormOptionsDto> GetFormOptionsAsync();
    Task<ReportDetailsDto> GetReportDetailsByIdAsync(Guid id);
}
