using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

namespace Korrekturmanagementsystem.Repositories;

public class ReportTypeRepository : BaseRepository<ReportType>, IReportTypeRepository
{
    public ReportTypeRepository(ApplicationDbContext context) : base(context) { }
}
