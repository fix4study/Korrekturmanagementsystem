using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportHistoryService
{
    Task<IEnumerable<ReportHistoryDto>> GetAllReportHistoriesByReportIdAsync(Guid reportId);
    Task AddReportHistoryAsync(CreateReportHistoryDto history);
}
