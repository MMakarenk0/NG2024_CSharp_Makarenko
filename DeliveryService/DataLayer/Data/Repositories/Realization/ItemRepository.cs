using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;

namespace DataLayer.Data.Repositories.Realization;

public class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(DeliveryServiceDbContext dbContext) : base(dbContext)
    {
    }
}
