using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem.Repositories;

public class ReportRepository : BaseRepository<Report>, IReportRepository
{
    public ReportRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Report?> GetByIdAsync(Guid id) =>
        await context.Reports
            .Include(r => r.Status)
            .Include(r => r.Priority)
            .Include(r => r.MaterialType)
            .Include(r => r.ReportType)
            .Include(r => r.Course)
            .Include(r => r.CreatedBy)
            .FirstOrDefaultAsync(r => r.Id == id);


    public async Task UpdateAsync() =>
        await context.SaveChangesAsync();
    

    public async Task<Guid?> GetCreatorIdByReportIdAsync(Guid id) =>
        await context.Reports
            .Where(r => r.Id == id)
            .Select(r => (Guid?)r.CreatedById)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Report>> GetAllAsync()
    {
        return await context.Reports
            .Include(r => r.Status)
            .Include(r => r.Priority)
            .Include(r => r.MaterialType)
            .Include(r => r.ReportType)
            .Include(r => r.Course)
            .Include(r => r.CreatedBy)
            .ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetAllByUserIdAsync(Guid userId) =>
        await context.Reports
            .Include(r => r.Status)
            .Include(r => r.Priority)
            .Include(r => r.MaterialType)
            .Include(r => r.ReportType)
            .Include(r => r.Course)
            .Include(r => r.CreatedBy)
            .Where(x => x.CreatedById == userId).ToListAsync();
}
