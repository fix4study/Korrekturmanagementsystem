using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Models;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using Korrekturmanagementsystem.Shared;
using Korrekturmanagementsystem.Providers.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class ReportService : IReportService
{
    private readonly IAttachmentProvider _attachmentProvider;
    private readonly IReportProvider _reportProvider;
    private readonly IReportTagProvider _reportTagProvider;
    private readonly IFileUploadProvider _fileUploadProvider;
    private readonly IReportHistoryProvider _reportHistoryProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ReportService(IReportProvider reportProvider,
        IReportTagProvider reportTagProvider,
        IAttachmentProvider attachmentProvider,
        IFileUploadProvider fileUploadProvider,
        IHttpContextAccessor httpContextAccessor,
        IReportHistoryProvider reportHistoryProvider)
    {
        _reportProvider = reportProvider;
        _reportTagProvider = reportTagProvider;
        _attachmentProvider = attachmentProvider;
        _fileUploadProvider = fileUploadProvider;
        _httpContextAccessor = httpContextAccessor;
        _reportHistoryProvider = reportHistoryProvider;
    }

    public async Task<EditReportModel> BuildEditReportViewModelAsync(Guid reportId)
    {
        var options = await _reportProvider.GetFormOptionsAsync();
        var details = await _reportProvider.GetReportDetailsByIdAsync(reportId);
        var attachments = await _attachmentProvider.GetByReportIdAsync(reportId);
        var reportTags = await _reportTagProvider.GetReportTagsByReportIdAsync(reportId);
        var reportHistory = await _reportHistoryProvider.GetAllReportHistoriesByReportIdAsync(reportId);

        var selectedTags = reportTags
            .Select(rt => new TagDto { Id = rt.TagId, Name = rt.TagName })
            .ToList();

        var dto = new UpdateReportDto
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

        return new EditReportModel
        {
            Report = dto,
            Options = options,
            Attachments = attachments.ToList(),
            SelectedTags = selectedTags,
            CreatedByUsername = details.CreatedByUsername ?? string.Empty,
            ReportHistory = reportHistory
        };
    }

    public async Task<Result> UpdateReportAsync(EditReportModel model, List<IBrowserFile> files)
    {
        var updateResult = await _reportProvider.UpdateReportByIdAsync(model.Report);

        if (!updateResult.IsSuccess)
        {
            return new Result { IsSuccess = false, Message = updateResult.Message ?? "Unbekannter Fehler" };
        }

        await AddReportHistoryEntry(model, model.StatusNote);

        await _reportTagProvider.UpdateReportTagsAsync(model.Report.Id, model.SelectedTags);

        var message = new StringBuilder("Meldung erfolgreich aktualisiert. ");

        if (files?.Count > 0)
        {
            var uploadResult = await _fileUploadProvider.UploadAsync(model.Report.Id, files);

            message.Append(uploadResult.Message ?? "Unbekannter Fehler.");
        }

        return new Result { IsSuccess = true, Message = message.ToString() ?? "Unbekannter Fehler" };
    }

    public async Task<ReportFormOptionsDto> GetFormOptionsAsync()
        => await _reportProvider.GetFormOptionsAsync();

    public async Task<Result> AddReportAsync(EditReportModel model, List<TagDto> selectedTags, List<IBrowserFile> files)
    {
        if (!_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false)
        {
            return new Result { IsSuccess = false, Message = "Sie sind nicht berechtigt eine Meldung zu erstellen." };
        }

        var report = new AddReportDto
        {
            Title = model.Report.Title,
            Description = model.Report.Description,
            ReportTypeId = model.Report.ReportTypeId,
            PriorityId = model.Report.PriorityId,
            MaterialTypeId = model.Report.MaterialTypeId,
            CourseId = model.Report.CourseId,
        };

        var reportId = await _reportProvider.AddReportAsync(report);

        if (reportId is null)
        {
            return new Result { IsSuccess = false, Message = "Fehler beim Erstellen der Meldung." };
        }

        if (selectedTags?.Count > 0)
        {
            var resportTags = selectedTags.Select(x => new ReportTagDto
            {
                ReportId = reportId.Value,
                TagId = x.Id
            }).ToList();

            await _reportTagProvider.InsertReportTagAsync(resportTags);
        }

        var message = new StringBuilder("Meldung erfolgreich gespeichert.");

        if (selectedTags?.Count > 0)
        {
            var uploadResult = await _fileUploadProvider.UploadAsync(reportId.Value, files);

            message.Append(uploadResult.Message ?? "Unbekannter Fehler.");
        }

        return new Result { IsSuccess = true, Message = message.ToString() ?? "Unbekannter Fehler" };
    }

    private async Task AddReportHistoryEntry(EditReportModel model, string statusNote)
    {
        var lastHistory = model.ReportHistory?
            .OrderByDescending(h => h.ChangedAt)
            .FirstOrDefault();

        var reportHistoryEntry = new CreateReportHistoryDto
        {
            ReportId = model.Report.Id,
            StatusId = model.Report.StatusId,
            Note = string.IsNullOrWhiteSpace(statusNote) ? null : statusNote
        };

        bool shouldAddHistory = false;

        if (lastHistory is null)
        {
            if (!string.IsNullOrWhiteSpace(statusNote) || model.Report.StatusId != 0)
            {
                shouldAddHistory = true;
            }
        }
        else
        {
            var lastHistoryStatusId = model.Options.Statuses
                .FirstOrDefault(x => x.Name == lastHistory.StatusName)?.Id ?? 0;

            bool statusChanged = model.Report.StatusId != lastHistoryStatusId;
            bool noteAdded = !string.IsNullOrWhiteSpace(statusNote);

            if (statusChanged || noteAdded)
            {
                shouldAddHistory = true;
            }
        }

        if (shouldAddHistory)
        {
            await _reportHistoryProvider.AddReportHistoryAsync(reportHistoryEntry);
        }
    }

}
