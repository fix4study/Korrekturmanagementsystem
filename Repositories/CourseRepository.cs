using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

namespace Korrekturmanagementsystem.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context) { }
}
