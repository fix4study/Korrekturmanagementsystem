using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Repositories.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class ReportOptionsService : IReportOptionsService
{
    private readonly IPriorityRepository _priorityRepository;
    private readonly IMaterialTypeRepository _materialTypeRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IReportTypeRepository _reportTypeRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly ITagRepository _tagRepository;
    public ReportOptionsService(IPriorityRepository priorityRepository,
        IMaterialTypeRepository materialTypeRepositorym,
        ICourseRepository courseRepository,
        IReportTypeRepository reportTypeRepository,
        IStatusRepository statusRepsoitory,
        ITagRepository tagRepository)
    {
        _priorityRepository = priorityRepository;
        _materialTypeRepository = materialTypeRepositorym;
        _courseRepository = courseRepository;
        _reportTypeRepository = reportTypeRepository;
        _statusRepository = statusRepsoitory;
        _tagRepository = tagRepository;
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
                .Select(s => new StatusDto { Id = s.Id, Name = s.Name }).ToList(),

            Tags = (await _tagRepository.GetAllAsync())
                .Select(t => new TagDto { Id = t.Id, Name = t.Name }).ToList()
        };
}
