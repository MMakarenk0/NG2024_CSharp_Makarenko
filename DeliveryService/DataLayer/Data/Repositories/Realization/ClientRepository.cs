using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;

namespace DataLayer.Data.Repositories.Realization;

public class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(DeliveryServiceDbContext dbContext) : base(dbContext)
    {
    }
}
