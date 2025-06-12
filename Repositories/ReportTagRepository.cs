using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem.Repositories;

public class ReportTagRepository : BaseRepository<ReportTag>, IReportTagRepository
{
    public ReportTagRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ReportTag>> GetReportTagsByReportIdAsync(Guid reportId) =>
        await context.ReportTags
            .Include(rt => rt.Tag)
            .Where(rt => rt.ReportId == reportId)
            .ToListAsync();


    public async Task<bool> DeleteByReportIdAsync(Guid reportId)
    {
        try
        {
            var tagsToDelete = context.ReportTags
                .Where(rt => rt.ReportId == reportId);

            context.ReportTags.RemoveRange(tagsToDelete);
            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
