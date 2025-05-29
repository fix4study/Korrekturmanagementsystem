using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem.Repositories;

public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Attachment>> GetAttachmentsByReportId(Guid reportId) =>
        await context.Attachments.Where(a => a.ReportId == reportId).ToListAsync();
}
