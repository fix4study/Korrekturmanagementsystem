using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Korrekturmanagementsystem.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    public IReportRepository Reports { get; }
    public IReportHistoryRepository ReportHistories { get; }
    public IReportTagRepository ReportTags { get; }
    public IAttachmentRepository Attachments { get; }
    public IUserRepository Users { get; }
    public IStatusRepository Statuses { get; }
    public ITagRepository Tags { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IReportRepository reports,
        IReportHistoryRepository reportHistories,
        IReportTagRepository reportTags,
        IAttachmentRepository attachments,
        IUserRepository users,
        IStatusRepository statuses,
        ITagRepository tags)
    {
        _context = context;
        Reports = reports;
        ReportHistories = reportHistories;
        ReportTags = reportTags;
        Attachments = attachments;
        Users = users;
        Statuses = statuses;
        Tags = tags;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
