using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;

namespace DataLayer.Data.Repositories.Realization;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(DeliveryServiceDbContext dbContext) : base(dbContext)
    {
    }
}
