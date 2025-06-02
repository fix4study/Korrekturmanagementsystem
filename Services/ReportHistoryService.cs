using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;
using Korrekturmanagementsystem.UnitOfWork;

namespace Korrekturmanagementsystem.Services;

public class ReportHistoryService : IReportHistoryService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public ReportHistoryService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ReportHistoryDto>> GetAllReportHistoriesByReportIdAsync(Guid reportId)
    {
        var histories = await _unitOfWork.ReportHistories.GetReportHistoriesByReportIdAsync(reportId);

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

    public async Task<Result> AddReportHistoryAsync(CreateReportHistoryDto history)
    {
        var userId = _currentUserService.GetCurrentUserId();

        if (userId is null)
        {
            return Result.Failure("Fehler bei der Ermittlung des aktuellen Nutzers.");
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

        var insertResult = await _unitOfWork.ReportHistories.InsertAsync(reportHistory);

        return insertResult.IsSuccess
            ? Result.Success()
            : Result.Failure("Das Hinzufügen der Historie ist fehlgeschlagen. Bitte versuchen Sie es erneut.");
    }
}
