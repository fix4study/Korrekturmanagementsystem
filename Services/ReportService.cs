using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Korrekturmanagementsystem.Services;

public class ReportService : IReportService
{
    private readonly IRepository<Report> _reportRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportService(IRepository<Report> repository, IHttpContextAccessor httpContextAccessor)
    {
        _reportRepository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> AddReportAsync(AddReportDto report)
    {
        var currentUser = GetCurrentUserId();

        if (currentUser == Guid.Empty)
        {
            return false;
        }

        new Report
        {
            Id = Guid.NewGuid(),
            Title = report.Title,
            Description = report.Description,
            ReportTypeId = report.ReportTypeId,
            Attachments = null,
            PriorityId = report.PriorityId,
            MaterialTypeId = report.MaterialTypeId,
            CourseId = report.CourseId,
            StatusId = 1, //Todo immer Status Offen
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedById = currentUser
        };

        return true;
    }

    public async Task<IEnumerable<ReportOverviewDto>> GetReportsOverviewAsync()
    {
        var reports = await _reportRepository.GetAsync(
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

    public async Task<ReportDetailsDto> GetReportDetailsByIdAsync(Guid id)
    {
        var report = await _reportRepository.GetByIdAsync(id);

        return new ReportDetailsDto
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
