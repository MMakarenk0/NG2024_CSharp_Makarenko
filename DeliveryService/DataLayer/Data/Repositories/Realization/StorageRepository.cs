using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;

namespace DataLayer.Data.Repositories.Realization;

public class StorageRepository : Repository<Storage>, IStorageRepository
{
    public StorageRepository(DeliveryServiceDbContext dbContext) : base(dbContext)
    {
    }
}
