using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;

namespace DataLayer.Data.Repositories.Realization;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(DeliveryServiceDbContext dbContext) : base(dbContext)
    {
    }
}
