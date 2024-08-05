using DataLayer.Data.Repositories.Interfaces;

namespace DataLayer.Data.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DeliveryServiceDbContext _dbContext;
    public IItemRepository ItemRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IItemCategoryRepository ItemCategoryRepository { get; }
    public IClientRepository ClientRepository { get; }
    public IEmployeeRepository EmployeeRepository { get; }
    public IManagerRepository ManagerRepository { get; }
    public IStorageRepository StorageRepository { get; }
    public UnitOfWork(
        DeliveryServiceDbContext dbContext,
        IItemRepository itemRepository,
        ICategoryRepository categoryRepository,
        IItemCategoryRepository itemCategoryRepository,
        IClientRepository clientRepository,
        IEmployeeRepository employeeRepository,
        IManagerRepository managerRepository,
        IStorageRepository storageRepository)
    {
        _dbContext = dbContext;
        ItemRepository = itemRepository;
        CategoryRepository = categoryRepository;
        ItemCategoryRepository = itemCategoryRepository;
        ClientRepository = clientRepository;
        EmployeeRepository = employeeRepository;
        ManagerRepository = managerRepository;
        StorageRepository = storageRepository;
    }
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
