using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;

namespace DataLayer.Data.Repositories.Realization;

public class ManagerRepository : Repository<Manager>, IManagerRepository
{
    public ManagerRepository(DeliveryServiceDbContext dbContext) : base(dbContext)
    {
    }
}
