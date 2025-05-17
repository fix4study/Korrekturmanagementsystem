using Korrekturmanagementsystem.Data.Entities;

namespace Korrekturmanagementsystem.Repositories.Interfaces;

public interface IReportHistoryRepository : IBaseRepository<ReportHistory>
{
    Task<IEnumerable<ReportHistory>> GetReportHistoriesByReportIdAsync(Guid reportId);
}
