using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

namespace Korrekturmanagementsystem.Repositories;

public class MaterialTypeRepository : BaseRepository<MaterialType>, IMaterialTypeRepository
{
    public MaterialTypeRepository(ApplicationDbContext context) : base(context) { }
}
