using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class ReportService : IReportService
{
    private readonly IRepository<Report> _reportRepository;
    public ReportService(IRepository<Report> repository)
    {
        _reportRepository = repository;
    }

    public async Task AddReport()
    {

    }

}
