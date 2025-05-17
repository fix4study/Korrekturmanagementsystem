using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Providers.Interfaces;

public interface IReportHistoryProvider
{
    Task<IEnumerable<ReportHistoryDto>> GetAllReportHistoriesByReportIdAsync(Guid reportId);
    Task AddReportHistoryAsync(CreateReportHistoryDto history);
}
