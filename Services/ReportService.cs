using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Models;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;
using Microsoft.AspNetCore.Components.Forms;

namespace Korrekturmanagementsystem.Services;

public class ReportService : IReportService
{
    private readonly IAttachmentService _attachmentService;
    private readonly IReportTagService _reportTagService;
    private readonly IFileUploadService _fileUploadService;
    private readonly IReportHistoryService _reportHistoryService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IReportOptionsService _reportOptionsService;
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IReportTagService reportTagService,
        IAttachmentService attachmentService,
        IFileUploadService fileUploadService,
        IReportHistoryService reportHistoryService,
        ICurrentUserService currentUserService,
        IReportOptionsService reportOptionsService,
        IUnitOfWork unitOfWork)
    {
        _reportTagService = reportTagService;
        _attachmentService = attachmentService;
        _fileUploadService = fileUploadService;
        _reportHistoryService = reportHistoryService;
        _currentUserService = currentUserService;
        _reportOptionsService = reportOptionsService;
        _unitOfWork = unitOfWork;
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

    public async Task<Result<Guid>> UpdateReportAsync(ReportModel model, List<IBrowserFile> files)
    {
        var reportDto = model.Report;

        var validationError = ValidateMandatoryFields(reportDto);
        if (!string.IsNullOrEmpty(validationError))
        {
            return Result<Guid>.Failure(validationError);
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var updateResult = await UpdateReportByIdAsync(reportDto);
            if (!updateResult.IsSuccess)
            {
                return await RollbackWithFailure(updateResult.Message);
            }

            var historyResult = await AddReportHistoryEntry(model, model.StatusNote);
            if (!historyResult.IsSuccess)
            {
                return await RollbackWithFailure(historyResult.Message);
            }

            var reportTagresult = await _reportTagService.UpdateReportTagsAsync(reportDto.Id, model.SelectedTags);
            if (!reportTagresult.IsSuccess)
            {
                return await RollbackWithFailure(reportTagresult.Message);
            }

            if (files?.Count > 0)
            {
                var uploadResult = await _fileUploadService.UploadAsync(reportDto.Id, files);
                if (!uploadResult.IsSuccess)
                {
                    return await RollbackWithFailure(uploadResult.Message);
                }
            }

            await _unitOfWork.CommitTransactionAsync();
            return Result<Guid>.Success(reportDto.Id, "Meldung erfolgreich aktualisiert.");
        }
        catch (Exception ex)
        {
            return await RollbackWithFailure("Bei der Aktualisierung der Meldung ist ein unerwarteter Fehler aufgetreten. Bitte probiere es erneut.");
        }
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

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var reportResult = await _unitOfWork.Reports.InsertAsync(newReport);
            if (!reportResult.IsSuccess)
            {
                return await RollbackWithFailure(reportResult.Message);
            }

            var historyResult = await AddInitialReportHistoryEntry(newReport.Id);
            if (!historyResult.IsSuccess)
            {
                return await RollbackWithFailure(historyResult.Message);
            }

            if (selectedTags?.Count > 0)
            {
                var reportTags = selectedTags.Select(x => new ReportTagDto
                {
                    ReportId = newReport.Id,
                    TagId = x.Id
                }).ToList();

                var reportTagResult = await _reportTagService.InsertReportTagAsync(reportTags);
                if (!reportTagResult.IsSuccess)
                {
                    return await RollbackWithFailure(reportTagResult.Message);
                }
            }

            if (files?.Count > 0)
            {
                var uploadResult = await _fileUploadService.UploadAsync(newReport.Id, files);
                if (!uploadResult.IsSuccess)
                {
                    return await RollbackWithFailure(uploadResult.Message);
                }
            }

            await _unitOfWork.CommitTransactionAsync();
            return Result<Guid>.Success(newReport.Id, "Meldung erfolgreich hinzugefügt.");
        }
        catch (Exception ex)
        {
            return await RollbackWithFailure("Beim Hinzufügen der Meldung ist ein unerwarteter Fehler aufgetreten. Bitte probiere es erneut.");
        }
    }

    public async Task<IEnumerable<ReportOverviewDto>> GetAllReportsAsync()
    {
        var reports = await _unitOfWork.Reports.GetAllAsync();

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

        var reports = await _unitOfWork.Reports.GetAllByUserIdAsync(userId.Value);

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
        => await _unitOfWork.Reports.GetCreatorIdByReportIdAsync(id);

    #region private

    private async Task<Result<Guid>> RollbackWithFailure(string? message)
    {
        await _unitOfWork.RollbackTransactionAsync();
        return Result<Guid>.Failure(message ?? "Unbekannter Fehler");
    }

    private async Task<Result> UpdateReportByIdAsync(ReportDto reportToUpdate)
    {
        var report = await _unitOfWork.Reports.GetByIdAsync(reportToUpdate.Id);

        if (report is null)
        {
            return Result.Failure("Meldung konnte nicht gefunden werden.");
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
            await _unitOfWork.Reports.UpdateAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Beim Aktualisieren der Meldung ist ein Fehler aufgetreten");
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
        var report = await _unitOfWork.Reports.GetByIdAsync(id);

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

    private async Task<Result> AddInitialReportHistoryEntry(Guid reportId)
    {
        var entry = new CreateReportHistoryDto
        {
            ReportId = reportId,
            StatusId = (int)Models.Enums.Status.Submitted,
            Note = string.Empty,
        };


        return await _reportHistoryService.AddReportHistoryAsync(entry);
    }

    private async Task<Result> AddReportHistoryEntry(ReportModel model, string? statusNote = "")
    {
        var lastHistory = model.ReportHistory?
            .OrderByDescending(h => h.ChangedAt)
            .FirstOrDefault();

        if (lastHistory is null)
        {
            return Result.Success();
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

        return statusChanged || noteAdded
            ? await _reportHistoryService.AddReportHistoryAsync(reportHistoryEntry)
            : Result.Success();
    }
    #endregion
}
