using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Models;
using Korrekturmanagementsystem.Models.Enums;
using Korrekturmanagementsystem.Providers.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;
using Microsoft.AspNetCore.Components.Forms;
using System.Text;

namespace Korrekturmanagementsystem.Services;

public class ReportService : IReportService
{
    private readonly IAttachmentProvider _attachmentProvider;
    private readonly IReportProvider _reportProvider;
    private readonly IReportTagProvider _reportTagProvider;
    private readonly IFileUploadProvider _fileUploadProvider;
    private readonly IReportHistoryProvider _reportHistoryProvider;
    private readonly ICurrentUserService _currentUserService;
    public ReportService(IReportProvider reportProvider,
        IReportTagProvider reportTagProvider,
        IAttachmentProvider attachmentProvider,
        IFileUploadProvider fileUploadProvider,
        IReportHistoryProvider reportHistoryProvider,
        ICurrentUserService currentUserService)
    {
        _reportProvider = reportProvider;
        _reportTagProvider = reportTagProvider;
        _attachmentProvider = attachmentProvider;
        _fileUploadProvider = fileUploadProvider;
        _reportHistoryProvider = reportHistoryProvider;
        _currentUserService = currentUserService;
    }

    public async Task<ReportModel> BuildEditReportViewModelAsync(Guid reportId)
    {
        var options = await _reportProvider.GetFormOptionsAsync();
        var details = await _reportProvider.GetReportDetailsByIdAsync(reportId);
        var attachments = await _attachmentProvider.GetByReportIdAsync(reportId);
        var reportTags = await _reportTagProvider.GetReportTagsByReportIdAsync(reportId);
        var reportHistory = await _reportHistoryProvider.GetAllReportHistoriesByReportIdAsync(reportId);

        var selectedTags = reportTags
            .Select(rt => new TagDto { Id = rt.TagId, Name = rt.TagName })
            .ToList();

        var dto = new ReportDto
        {
            Id = reportId,
            Title = details.Title,
            Description = details.Description,
            ReportTypeId = details.ReportType.Id,
            PriorityId = details.Priority.Id,
            MaterialTypeId = details.MaterialType.Id,
            CourseId = details.Course?.Id,
            StatusId = details.Status.Id,
            TagIds = selectedTags.Select(t => t.Id).ToList(),
        };

        return new ReportModel
        {
            Report = dto,
            Options = options,
            Attachments = attachments.ToList(),
            SelectedTags = selectedTags,
            CreatedByUsername = details.CreatedByUsername ?? string.Empty,
            ReportHistory = reportHistory
        };
    }

    public async Task<Result> UpdateReportAsync(ReportModel model, List<IBrowserFile> files)
    {
        var reportDto = model.Report;

        var validationError = ValidateMandatoryFields(reportDto);

        if (validationError is not null)
        {
            return new Result { IsSuccess = false, Message = validationError };
        }

        var updateResult = await _reportProvider.UpdateReportByIdAsync(reportDto);

        if (!updateResult.IsSuccess)
        {
            return new Result { IsSuccess = false, Message = updateResult.Message ?? "Unbekannter Fehler" };
        }

        await AddReportHistoryEntry(model, model.StatusNote);

        await _reportTagProvider.UpdateReportTagsAsync(reportDto.Id, model.SelectedTags);

        var message = new StringBuilder("Meldung erfolgreich aktualisiert. ");

        if (files?.Count > 0)
        {
            var uploadResult = await _fileUploadProvider.UploadAsync(reportDto.Id, files);

            message.Append(uploadResult.Message ?? "Unbekannter Fehler.");
        }

        return new Result { IsSuccess = true, Message = message.ToString() ?? "Unbekannter Fehler" };
    }

    public async Task<ReportFormOptionsDto> GetFormOptionsAsync()
        => await _reportProvider.GetFormOptionsAsync();

    public async Task<Result<Guid>> AddReportAsync(ReportModel model, List<TagDto> selectedTags, List<IBrowserFile> files)
    {
        var reportDto = model.Report;

        var validationError = ValidateMandatoryFields(reportDto);
        if (validationError is not null)
        {
            return Result<Guid>.Failure(validationError);
        }

        var report = new AddReportDto
        {
            Title = reportDto.Title,
            Description = reportDto.Description,
            ReportTypeId = reportDto.ReportTypeId!.Value,
            PriorityId = reportDto.PriorityId!.Value,
            MaterialTypeId = reportDto.MaterialTypeId!.Value,
            CourseId = reportDto.CourseId,
        };

        var userId = _currentUserService.GetCurrentUserId();
        if (userId is null)
        {
            return Result<Guid>.Failure("Etwas ist bei der Authentifizierung schiefgelaufen.");
        }

        var reportId = await _reportProvider.AddReportAsync(report, userId.Value);
        if (reportId is null)
        {
            return Result<Guid>.Failure("Fehler beim Erstellen der Meldung.");
        }

        await AddInitialReportHistoryEntry(reportId.Value);

        if (selectedTags?.Count > 0)
        {
            var reportTags = selectedTags.Select(x => new ReportTagDto
            {
                ReportId = reportId.Value,
                TagId = x.Id
            }).ToList();

            await _reportTagProvider.InsertReportTagAsync(reportTags);
        }

        var message = new StringBuilder("Meldung erfolgreich hinzugefügt.");

        if (selectedTags?.Count > 0)
        {
            var uploadResult = await _fileUploadProvider.UploadAsync(reportId.Value, files);
            message.Append(uploadResult.Message ?? "Unbekannter Fehler beim Upload.");
        }

        return Result<Guid>.Success(reportId.Value, message.ToString());
    }

    public async Task<IEnumerable<ReportOverviewDto>> GetAllReportsAsync()
        => await _reportProvider.GetReportsOverviewAsync();

    public async Task<IEnumerable<ReportOverviewDto>> GetAllReportByUserIdAsync()
    {
        var userId = _currentUserService.GetCurrentUserId();

        if (userId is null)
        {
            return Enumerable.Empty<ReportOverviewDto>();
        }

        return await _reportProvider.GetAllReportByUserIdAsync(userId.Value);
    }

    #region private
    private string? ValidateMandatoryFields(ReportDto report)
    {
        if (string.IsNullOrWhiteSpace(report.Title))
        {
            return "Bitte geben Sie einen Titel an.";
        }
        if (report.ReportTypeId == null)
        {
            return "Bitte wählen Sie einen Meldungstyp.";
        }
        if (report.PriorityId == null)
        {
            return "Bitte wählen Sie eine Priorität.";
        }
        if (report.MaterialTypeId == null)
        {
            return "Bitte wählen Sie ein Material.";
        }

        return null;
    }

    private async Task AddInitialReportHistoryEntry(Guid reportId)
    {
        var entry = new CreateReportHistoryDto
        {
            ReportId = reportId,
            StatusId = (int)Status.Eingereicht,
            Note = string.Empty,
        };

        await _reportHistoryProvider.AddReportHistoryAsync(entry);
    }

    private async Task AddReportHistoryEntry(ReportModel model, string? statusNote = "")
    {
        var lastHistory = model.ReportHistory?
            .OrderByDescending(h => h.ChangedAt)
            .FirstOrDefault();

        if (lastHistory is null)
        {
            return;
        }

        var reportHistoryEntry = new CreateReportHistoryDto
        {
            ReportId = model.Report.Id,
            StatusId = model.Report.StatusId,
            Note = string.IsNullOrWhiteSpace(statusNote) ? null : statusNote
        };

        var lastHistoryStatusId = model.Options.Statuses
            .FirstOrDefault(x => x.Name == lastHistory.StatusName)?.Id ?? 0;

        bool statusChanged = model.Report.StatusId != lastHistoryStatusId;
        bool noteAdded = !string.IsNullOrWhiteSpace(statusNote);

        if (statusChanged || noteAdded)
        {
            await _reportHistoryProvider.AddReportHistoryAsync(reportHistoryEntry);
        }
    }
    #endregion
}
