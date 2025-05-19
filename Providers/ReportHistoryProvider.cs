using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Providers.Interfaces;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Providers;

public class ReportHistoryProvider : IReportHistoryProvider
{
    private readonly IReportHistoryRepository _reportHistoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public ReportHistoryProvider(IReportHistoryRepository reportHistoryRepository, ICurrentUserService currentUserService)
    {
        _reportHistoryRepository = reportHistoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ReportHistoryDto>> GetAllReportHistoriesByReportIdAsync(Guid reportId)
    {
        var histories = await _reportHistoryRepository.GetReportHistoriesByReportIdAsync(reportId);

        var historyDtos = new List<ReportHistoryDto>();
        foreach (var history in histories)
        {
            var reportHistory = new ReportHistoryDto
            {
                StatusName = history.Status.Name,
                ChangedByName = history.ChangedBy.Username,
                ChangedAt = history.ChangedAt,
                Note = history.Note
            };

            historyDtos.Add(reportHistory);
        }

        return historyDtos;
    }

    public async Task AddReportHistoryAsync(CreateReportHistoryDto history)
    {
        var userId = _currentUserService.GetCurrentUserId();

        if (userId is null)
        {
            return;
        }

        var reportHistory = new ReportHistory
        {
            Id = Guid.NewGuid(),
            ReportId = history.ReportId,
            StatusId = history.StatusId,
            ChangedById = userId.Value,
            ChangedAt = DateTime.UtcNow,
            Note = history.Note
        };

        await _reportHistoryRepository.InsertAsync(reportHistory);
    }
}
