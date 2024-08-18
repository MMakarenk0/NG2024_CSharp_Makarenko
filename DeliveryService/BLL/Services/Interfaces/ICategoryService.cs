using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;

namespace BLL.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryReadModel>> GetAllAsync();
    Task<Guid> AddAsync(SaveCategoryModel model);
    Task<Guid> UpdateAsync(UpdateCategoryModel model);
    Task DeleteAsync(Guid id);
    Task<CategoryReadModel> GetByIdAsync(Guid id);
}
