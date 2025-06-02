using Korrekturmanagementsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

public interface IUnitOfWork
{
    IReportRepository Reports { get; }
    IReportHistoryRepository ReportHistories { get; }
    IReportTagRepository ReportTags { get; }
    IAttachmentRepository Attachments { get; }
    IUserRepository Users { get; }
    IStatusRepository Statuses { get; }
    ITagRepository Tags { get; }
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
