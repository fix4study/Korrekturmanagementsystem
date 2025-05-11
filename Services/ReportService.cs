using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Korrekturmanagementsystem.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IBaseRepository<Priority> _priorityRepository;
    private readonly IBaseRepository<MaterialType> _materialTypeRepository;
    private readonly IBaseRepository<Course> _courseRepository;
    private readonly IBaseRepository<ReportType> _reportTypeRepository;
    private readonly IBaseRepository<Status> _statusRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportService(IReportRepository reportRepository, IHttpContextAccessor httpContextAccessor,
        IBaseRepository<Priority> priorityRepository,
        IBaseRepository<MaterialType> materialTypeRepositorym,
        IBaseRepository<Course> courseRepository,
        IBaseRepository<ReportType> reportTypeRepository,
        IBaseRepository<Status> statusRepsoitory)
    {
        _reportRepository = reportRepository;
        _httpContextAccessor = httpContextAccessor;
        _priorityRepository = priorityRepository;
        _materialTypeRepository = materialTypeRepositorym;
        _courseRepository = courseRepository;
        _reportTypeRepository = reportTypeRepository;
        _statusRepository = statusRepsoitory;
    }

    public async Task<Guid?> AddReportAsync(AddReportDto report)
    {
        try
        {
            var currentUser = GetCurrentUserId();

            if (currentUser == Guid.Empty)
            {
                return null;
            }

            var newReport = new Report
            {
                Id = Guid.NewGuid(),
                Title = report.Title,
                Description = report.Description,
                ReportTypeId = report.ReportTypeId,
                PriorityId = report.PriorityId,
                MaterialTypeId = report.MaterialTypeId,
                CourseId = report.CourseId,
                StatusId = 1, //Todo immer Status Offen
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedById = currentUser
            };

            await _reportRepository.InsertAsync(newReport);

            return newReport.Id;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

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
            .Select(c => new StatusDto { Id = c.Id, Name = c.Name }).ToList(),
        };

    public async Task<IEnumerable<ReportOverviewDto>> GetReportsOverviewAsync()
    {
        var reports = await _reportRepository.GetAllAsync(
            includes: new Expression<Func<Report, object>>[]
            {
                kms => kms.Priority,
                kms => kms.Status
            });

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

    public async Task<Result> UpdateReportByIdAsync(UpdateReportDto reportToUpdate)
    {
        if (!_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false)
        {
            return Result.Failure("Sie sind nicht angemeldet. Bitte loggen Sie sich ein.");
        }

        var report = await _reportRepository.GetByIdAsync(reportToUpdate.Id);

        if (report is null)
        {
            return Result.Failure("Meldung wurde nicht gefunden");
        }

        report.Title = reportToUpdate.Title;
        report.Description = reportToUpdate.Description;
        report.ReportTypeId = reportToUpdate.ReportTypeId;
        report.PriorityId = reportToUpdate.PriorityId;
        report.MaterialTypeId = reportToUpdate.MaterialTypeId;
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

    private Guid GetCurrentUserId()
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException("Ungültige Benutzer-ID im Token.");
        }

        return userId;
    }
}
