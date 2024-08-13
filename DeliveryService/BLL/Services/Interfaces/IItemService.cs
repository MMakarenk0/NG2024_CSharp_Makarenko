using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;

namespace BLL.Services.Interfaces;
public interface IItemService
{
    Task<IEnumerable<ItemReadModel>> GetAllAsync();
    Task<Guid> AddAsync(SaveItemModel model);
    Task<Guid> UpdateAsync(UpdateItemModel model);
    Task DeleteAsync(Guid id);
    Task<ItemReadModel> GetByIdAsync(Guid id);
}
