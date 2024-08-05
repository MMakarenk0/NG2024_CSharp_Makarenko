using DataLayer.Data.Repositories.Interfaces;

namespace DataLayer.Data.Infrastructure;

public interface IUnitOfWork
{
    IItemRepository ItemRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IItemCategoryRepository ItemCategoryRepository { get; }
    IClientRepository ClientRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IManagerRepository ManagerRepository { get; }
    IStorageRepository StorageRepository { get; }

    Task SaveChangesAsync();
}
