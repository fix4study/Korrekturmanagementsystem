using Korrekturmanagementsystem.Data.Entities;

namespace Korrekturmanagementsystem.Repositories.Interfaces;

public interface IReportTagRepository : IBaseRepository<ReportTag>
{
    Task<IEnumerable<ReportTag>> GetReportTagsByReportIdAsync(Guid reportId);
    Task<bool> DeleteByReportIdAsync(Guid reportId);
}
