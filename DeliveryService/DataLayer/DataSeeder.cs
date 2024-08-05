using DataLayer.Data.Infrastructure;
using DataLayer.Entities;

namespace DataLayer;

public class DataSeeder
{
    private readonly IUnitOfWork _uof;

    public DataSeeder(IUnitOfWork uof)
    {
        _uof = uof;
    }

    public async Task Seed()
    {
        var clientId1 = Guid.NewGuid();
        var clientId2 = Guid.NewGuid();
        var directorId = Guid.NewGuid();
        var storageId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        var clients = new List<Client>
        {
            new Client { Id = clientId1, Name = "John", Phone = "1111" },
            new Client { Id = clientId2, Name = "Kyle", Phone = "2222" }
        };

        var categories = new List<Category>
        {
            new Category { Id = categoryId, Description = "Electronics" }
        };

        var manager = new Manager
        {
            Id = directorId,
            Name = "Michael",
            StorageId = storageId
        };

        var storage = new Storage
        {
            Id = storageId,
            Address = "456 Maple Ave, Apt 12, Newtown, USA",
            DirectorId = directorId,
            Number = 235
        };

        var item = new Item
        {
            Id = itemId,
            Description = "Laptop ACER Nitro 5",
            Weight = 2.5f,
            Price = 1200m,
            StorageId = storageId,
            SenderId = clientId1,
            ReceiverId = clientId2
        };

        var itemCategory = new ItemCategory
        {
            ItemId = itemId,
            CategoryId = categoryId
        };

        foreach (var client in clients)
        {
            await _uof.ClientRepository.Create(client);
        }

        foreach (var category in categories)
        {
            await _uof.CategoryRepository.Create(category);
        }

        await _uof.ManagerRepository.Create(manager);
        await _uof.StorageRepository.Create(storage);
        await _uof.ItemRepository.Create(item);
        await _uof.ItemCategoryRepository.Create(itemCategory);

        await _uof.SaveChangesAsync();
    }
}
