using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;

namespace BLL.Services.Interfaces;

public interface IManagerService
{
    Task<IEnumerable<ManagerReadModel>> GetAllAsync();
    Task<Guid> AddAsync(SaveManagerModel model);
    Task<Guid> UpdateAsync(UpdateManagerModel model);
    Task DeleteAsync(Guid id);
    Task<ManagerReadModel> GetByIdAsync(Guid id);
}
