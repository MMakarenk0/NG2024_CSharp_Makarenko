using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;

namespace BLL.Services.Interfaces;

public interface IStorageService
{
    Task<IEnumerable<StorageReadModel>> GetAllAsync();
    Task<Guid> AddAsync(SaveStorageModel model);
    Task<Guid> UpdateAsync(UpdateStorageModel model);
    Task DeleteAsync(Guid id);
    Task<StorageReadModel> GetByIdAsync(Guid id);
}
