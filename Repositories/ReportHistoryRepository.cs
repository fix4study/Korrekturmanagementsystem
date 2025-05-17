using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem.Repositories;

public class ReportHistoryRepository : BaseRepository<ReportHistory>, IReportHistoryRepository
{
    public ReportHistoryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ReportHistory>> GetReportHistoriesByReportIdAsync(Guid reportId)
    {
        return await context.ReportHistories
            .Where(x => x.ReportId == reportId)
            .Include(r => r.Status)
            .Include(r => r.ChangedBy)
            .ToListAsync();
    }
}
