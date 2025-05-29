using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IMaterialProvider
{
    Task AddMaterialTypeAsync(string name);

    Task<IEnumerable<MaterialTypeDto>> GetMaterialTypesAsync();
}
