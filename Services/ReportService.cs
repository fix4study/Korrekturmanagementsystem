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
    private readonly IRepository<Priority> _priorityRepository;
    private readonly IRepository<MaterialType> _materialTypeRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<ReportType> _reportTypeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportService(IRepository<Report> reportRepository, IHttpContextAccessor httpContextAccessor,
        IRepository<Priority> priorityRepository,
    IRepository<MaterialType> materialTypeRepositorym,
    IRepository<Course> courseRepository,
    IRepository<ReportType> reportTypeRepository)
    {
        _reportRepository = reportRepository;
        _httpContextAccessor = httpContextAccessor;
        _priorityRepository = priorityRepository;
        _materialTypeRepository = materialTypeRepositorym;
        _courseRepository = courseRepository;
        _reportTypeRepository = reportTypeRepository;
    }

    public async Task<bool> AddReportAsync(AddReportDto report)
    {
        try
        {
            var currentUser = GetCurrentUserId();

            if (currentUser == Guid.Empty)
            {
                return false;
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

            return true;
        }
        catch (Exception ex)
        {
            return false;
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
