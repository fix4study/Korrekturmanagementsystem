using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class MaterialProvider : IMaterialProvider
{
    private readonly IBaseRepository<MaterialType> _repository;
    public MaterialProvider(IBaseRepository<MaterialType> repository)
    {
        _repository = repository;
    }

    public async Task AddMaterialTypeAsync(string name)
    {
        var materialType = new MaterialType
        {
            Name = name
        };

        await _repository.InsertAsync(materialType);
    }

    public async Task<IEnumerable<MaterialTypeDto>> GetMaterialTypesAsync()
    {
        var materialTypes = await _repository.GetAllAsync();

        return materialTypes.Select(materialTypes => new MaterialTypeDto
        {
            Id = materialTypes.Id,
            Name = materialTypes.Name
        });
    }
}
