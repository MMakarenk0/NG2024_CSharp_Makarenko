using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;

namespace BLL.Services.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientReadModel>> GetAllAsync();
    Task<Guid> AddAsync(SaveClientModel model);
    Task<Guid> UpdateAsync(UpdateClientModel model);
    Task DeleteAsync(Guid id);
    Task<ClientReadModel> GetByIdAsync(Guid id);
}
