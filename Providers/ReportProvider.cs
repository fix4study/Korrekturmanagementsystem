using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services;

public class ReportProvider : IReportProvider
{
    private readonly IReportRepository _reportRepository;
    private readonly IPriorityRepository _priorityRepository;
    private readonly IMaterialTypeRepository _materialTypeRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IReportTypeRepository _reportTypeRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly ITagRepository _tagRepository;

    public ReportProvider(IReportRepository reportRepository,
        IPriorityRepository priorityRepository,
        IMaterialTypeRepository materialTypeRepositorym,
        ICourseRepository courseRepository,
        IReportTypeRepository reportTypeRepository,
        IStatusRepository statusRepsoitory,
        ITagRepository tagRepository)
    {
        _reportRepository = reportRepository;
        _priorityRepository = priorityRepository;
        _materialTypeRepository = materialTypeRepositorym;
        _courseRepository = courseRepository;
        _reportTypeRepository = reportTypeRepository;
        _statusRepository = statusRepsoitory;
        _tagRepository = tagRepository;
    }

    public async Task<Guid?> AddReportAsync(AddReportDto report, Guid userId)
    {
        try
        {
            var newReport = new Report
            {
                Id = Guid.NewGuid(),
                Title = report.Title,
                Description = report.Description,
                ReportTypeId = report.ReportTypeId,
                PriorityId = (int)Models.Enums.Priority.Medium,
                MaterialTypeId = report.MaterialTypeId,
                CourseId = report.CourseId,
                StatusId = (int)Models.Enums.Status.Submitted,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            await _reportRepository.InsertAsync(newReport);

            return newReport.Id;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<Guid?> GetCreatorUserIdByReportIdAsync(Guid id) =>
        await _reportRepository.GetCreatorIdByReportIdAsync(id);

    public async Task<ReportFormOptionsDto> GetFormOptionsAsync() =>
        new ReportFormOptionsDto
        {
            ReportTypes = (await _reportTypeRepository.GetAllAsync())
            .Select(r => new ReportTypeDto { Id = r.Id, Name = r.Name }).ToList(),

            Priorities = (await _priorityRepository.GetAllAsync())
            .Select(p => new PriorityDto { Id = p.Id, Name = p.Name }).ToList(),

            MaterialTypes = (await _materialTypeRepository.GetAllAsync())
            .Select(m => new MaterialTypeDto { Id = m.Id, Name = m.Name }).ToList(),

            Courses = (await _courseRepository.GetAllAsync())
            .Select(c => new CourseDto { Id = c.Id, Name = c.Name, Code = c.Code }).ToList(),

            Statuses = (await _statusRepository.GetAllAsync())
            .Select(s => new StatusDto { Id = s.Id, Name = s.Name }).ToList(),

            Tags = (await _tagRepository.GetAllAsync())
            .Select(t => new TagDto { Id = t.Id, Name = t.Name }).ToList()
        };

    public async Task<IEnumerable<ReportOverviewDto>> GetReportsOverviewAsync()
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

    public async Task<IEnumerable<ReportOverviewDto>> GetAllReportByUserIdAsync(Guid userId)
    {
        var reports = await _reportRepository.GetAllByUserIdAsync(userId);

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

    public async Task<Result> UpdateReportByIdAsync(ReportDto reportToUpdate)
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

    public async Task<ReportDetailsDto> GetReportDetailsByIdAsync(Guid id)
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
}
