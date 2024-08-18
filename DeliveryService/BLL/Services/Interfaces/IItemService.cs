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
    Task<IEnumerable<ItemReadModel>> GetBySenderPhone(string senderPhone);
    Task<IEnumerable<ItemReadModel>> GetByRecieverPhone(string receiverPhone);
    Task<IEnumerable<ItemReadModel>> GetByStorage(Guid storageId);
    Task<IEnumerable<ItemReadModel>> GetByFilters(string? description, float? minWeight, float? maxWeight, List<Guid> categoryIds, DateTime? startDate, DateTime? endDate);
    Task<Guid> UpdateItemStatus(Guid itemId, bool? newStatus);
}
