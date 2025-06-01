using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Models;
using Korrekturmanagementsystem.Models.Enums;
using Korrekturmanagementsystem.Repositories;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace Korrekturmanagementsystem.Services;

public class ReportService : IReportService
{
    private readonly IAttachmentService _attachmentService;
    private readonly IReportRepository _reportRepository;
    private readonly IReportTagService _reportTagService;
    private readonly IFileUploadService _fileUploadService;
    private readonly IReportHistoryService _reportHistoryService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IReportOptionsService _reportOptionsService;
    public ReportService(IReportRepository reportRepository,
        IReportTagService reportTagService,
        IAttachmentService attachmentService,
        IFileUploadService fileUploadService,
        IReportHistoryService reportHistoryService,
        ICurrentUserService currentUserService,
        IReportOptionsService reportOptionsService)
    {
        _reportRepository = reportRepository;
        _reportTagService = reportTagService;
        _attachmentService = attachmentService;
        _fileUploadService = fileUploadService;
        _reportHistoryService = reportHistoryService;
        _currentUserService = currentUserService;
        _reportOptionsService = reportOptionsService;
    }

    public async Task<ReportModel?> BuildEditReportViewModelAsync(Guid reportId)
    {
        var options = await _reportOptionsService.GetFormOptionsAsync();
        var details = await GetReportDetailsByIdAsync(reportId);
        if (details is null)
        {
            return null;
        }

        var attachments = await _attachmentService.GetByReportIdAsync(reportId);
        var reportTags = await _reportTagService.GetReportTagsByReportIdAsync(reportId);
        var reportHistory = await _reportHistoryService.GetAllReportHistoriesByReportIdAsync(reportId);

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

        if (!string.IsNullOrEmpty(validationError))
        {
            return new Result { IsSuccess = false, Message = validationError };
        }

        var updateResult = await UpdateReportByIdAsync(reportDto);

        if (!updateResult.IsSuccess)
        {
            return new Result { IsSuccess = false, Message = updateResult.Message };
        }

        await AddReportHistoryEntry(model, model.StatusNote);

        await _reportTagService.UpdateReportTagsAsync(reportDto.Id, model.SelectedTags);

        var message = "Meldung erfolgreich aktualisiert.";

        if (files?.Count > 0)
        {
            var uploadResult = await _fileUploadService.UploadAsync(reportDto.Id, files);

            message += $" {uploadResult.Message}";
        }

        return new Result { IsSuccess = true, Message = message.ToString() };
    }

    public async Task<Result<Guid>> AddReportAsync(ReportModel model, List<TagDto> selectedTags, List<IBrowserFile> files)
    {
        if (model is null || model.Report is null || model.Report.ReportTypeId is null || model.Report.MaterialTypeId is null)
        {
            return Result<Guid>.Failure("Ungültiges Eingabe.");
        }

        var validationError = ValidateMandatoryFields(model.Report);

        if (!string.IsNullOrEmpty(validationError))
        {
            return Result<Guid>.Failure(validationError);
        }

        var userId = _currentUserService.GetCurrentUserId();
        if (userId is null)
        {
            return Result<Guid>.Failure("Etwas ist bei der Authentifizierung schiefgelaufen.");
        }

        var newReport = new Report
        {
            Id = Guid.NewGuid(),
            Title = model.Report.Title,
            Description = model.Report.Description,
            ReportTypeId = (int)model.Report.ReportTypeId!,
            PriorityId = (int)Models.Enums.Priority.Medium,
            MaterialTypeId = (int)model.Report.MaterialTypeId,
            CourseId = model.Report.CourseId,
            StatusId = (int)Models.Enums.Status.Submitted,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedById = userId.Value
        };

        await _reportRepository.InsertAsync(newReport);

        await AddInitialReportHistoryEntry(newReport.Id);

        if (selectedTags?.Count > 0)
        {
            var reportTags = selectedTags.Select(x => new ReportTagDto
            {
                ReportId = newReport.Id,
                TagId = x.Id
            }).ToList();

            await _reportTagService.InsertReportTagAsync(reportTags);
        }

        var message = "Meldung erfolgreich hinzugefügt.";

        if (files?.Count > 0)
        {
            var uploadResult = await _fileUploadService.UploadAsync(newReport.Id, files);
            message += $" {uploadResult.Message}";
        }

        return Result<Guid>.Success(newReport.Id, message.ToString());
    }

    public async Task<IEnumerable<ReportOverviewDto>> GetAllReportsAsync()
    {
        var reports = await _reportRepository.GetAllAsync();

        return reports.Select(report => new ReportOverviewDto
        {
            Id = report.Id,
            Title = report.Title,
            StatusName = report.Status.Name,
            PriorityName = report.Priority.Name,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt
        });
    }

    public async Task<IEnumerable<ReportOverviewDto>> GetAllReportByUserIdAsync()
    {
        var userId = _currentUserService.GetCurrentUserId();

        if (userId is null)
        {
            return Enumerable.Empty<ReportOverviewDto>();
        }

        var reports = await _reportRepository.GetAllByUserIdAsync(userId.Value);

        return reports.Select(report => new ReportOverviewDto
        {
            Id = report.Id,
            Title = report.Title,
            StatusName = report.Status.Name,
            PriorityName = report.Priority.Name,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt
        });
    }

    public async Task<Guid?> GetCreatorUserIdByReportIdAsync(Guid id)
        => await _reportRepository.GetCreatorIdByReportIdAsync(id);

    #region private
    private async Task<Result> UpdateReportByIdAsync(ReportDto reportToUpdate)
    {
        var report = await _reportRepository.GetByIdAsync(reportToUpdate.Id);

        if (report is null)
        {
            return Result.Failure("Meldung wurde nicht gefunden");
        }

        report.Title = reportToUpdate.Title;
        report.Description = reportToUpdate.Description;
        report.ReportTypeId = reportToUpdate.ReportTypeId!.Value;
        report.PriorityId = reportToUpdate.PriorityId!.Value;
        report.MaterialTypeId = reportToUpdate.MaterialTypeId!.Value;
        report.CourseId = reportToUpdate.CourseId;
        report.StatusId = reportToUpdate.StatusId;
        report.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _reportRepository.UpdateAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Beim Speichern ist ein technischer Fehler aufgetreten");
        }
    }

    private static string ValidateMandatoryFields(ReportDto report)
    {
        if (string.IsNullOrWhiteSpace(report.Title))
        {
            return "Bitte geben Sie einen Titel an.";
        }
        if (report.ReportTypeId == null)
        {
            return "Bitte wählen Sie einen Meldungstyp.";
        }
        if (report.MaterialTypeId == null)
        {
            return "Bitte wählen Sie ein Material.";
        }

        return string.Empty;
    }

    private async Task<ReportDetailsDto> GetReportDetailsByIdAsync(Guid id)
    {
        var report = await _reportRepository.GetByIdAsync(id);

        var reportDetails = new ReportDetailsDto
        {
            Id = report.Id,
            Title = report.Title,
            Description = report.Description,
            ReportType = new ReportTypeDto
            {
                Id = report.ReportType.Id,
                Name = report.ReportType.Name
            },
            Status = new StatusDto
            {
                Id = report.Status.Id,
                Name = report.Status.Name
            },
            Priority = new PriorityDto
            {
                Id = report.Priority.Id,
                Name = report.Priority.Name
            },
            MaterialType = new MaterialTypeDto
            {
                Id = report.MaterialType.Id,
                Name = report.MaterialType.Name
            },
            Course = report.Course != null
            ? new CourseDto
            {
                Id = report.Course.Id,
                Name = report.Course.Name,
                Code = report.Course.Code
            }
            : null,
            CreatedByUsername = report.CreatedBy.Username,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt
        };

        return reportDetails;
    }

    private async Task AddInitialReportHistoryEntry(Guid reportId)
    {
        var entry = new CreateReportHistoryDto
        {
            ReportId = reportId,
            StatusId = (int)Models.Enums.Status.Submitted,
            Note = string.Empty,
        };

        await _reportHistoryService.AddReportHistoryAsync(entry);
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
            await _reportHistoryService.AddReportHistoryAsync(reportHistoryEntry);
        }
    }
    #endregion
}
