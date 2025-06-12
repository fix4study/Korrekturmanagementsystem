using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportHistoryService
{
    Task<IEnumerable<ReportHistoryDto>> GetAllReportHistoriesByReportIdAsync(Guid reportId);
    Task<Result> AddReportHistoryAsync(CreateReportHistoryDto history);
}
