using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.DataLayerTests;

public class ItemRepositoryTests
{
    private readonly ItemRepository _itemRepository;
    private readonly DeliveryServiceDbContext _dbContext;

    public ItemRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DeliveryServiceDbContext(options);

        _itemRepository = new ItemRepository(_dbContext);
    }
    [Fact]
    public void GetAll_ShouldReturnAllItems()
    {
        // Arrange
        var item1 = new Item
        {
            Id = Guid.NewGuid(),
            Description = "Item1",
            Date = DateTime.Now,
            Price = 100m,
            Weight = 1
        };
        var item2 = new Item
        {
            Id = Guid.NewGuid(),
            Description = "Item2",
            Date = DateTime.Now,
            Price = 100m,
            Weight = 1
        };
        _dbContext.Items.AddRange(item1, item2);
        _dbContext.SaveChanges();

        // Act
        var result = _itemRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredItems()
    {
        // Arrange
        var item1 = new Item
        {
            Id = Guid.NewGuid(),
            Description = "Item1",
            Date = DateTime.Now,
            Price = 100m,
            Weight = 1
        };
        var item2 = new Item
        {
            Id = Guid.NewGuid(),
            Description = "Item1",
            Date = DateTime.Now,
            Price = 123m,
            Weight = 1
        };
        var item3 = new Item
        {
            Id = Guid.NewGuid(),
            Description = "Item1",
            Date = DateTime.Now,
            Price = 1000m,
            Weight = 1
        };
        _dbContext.Items.AddRange(item1, item2, item3);
        _dbContext.SaveChanges();

        // Act
        var result = await _itemRepository.GetAllAsync(i => i.Price <= 100);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task Create_ShouldAddItemToDbSet()
    {
        // Arrange
        var item = new Item
        {
            Id = Guid.NewGuid(),
            Description = "New Item",
            Date = DateTime.Now,
            Price = 100m,
            Weight = 1
        };

        // Act
        await _itemRepository.Create(item);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdItem = await _dbContext.Items.FindAsync(item.Id);
        Assert.NotNull(createdItem);
        Assert.Equal("New Item", createdItem.Description);
    }

    [Fact]
    public async Task Delete_ShouldDeleteItem_WhenItemExists()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = new Item
        {
            Id = itemId,
            Description = "New Item",
            Date = DateTime.Now,
            Price = 100m,
            Weight = 1
        };
        await _itemRepository.Create(item);

        // Act
        await _itemRepository.Delete(itemId);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdItem = await _dbContext.Items.FindAsync(itemId);
        Assert.Null(createdItem);
    }

    [Fact]
    public async Task Find_ShouldReturnItem_WhenItemExists()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = new Item
        {
            Id = itemId,
            Description = "New Item",
            Date = DateTime.Now,
            Price = 100m,
            Weight = 1
        };
        await _itemRepository.Create(item);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _itemRepository.Find(itemId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Item", result.Description);
    }

    [Fact]
    public async Task Update_ShouldModifyItem_WhenItemExists()
    {
        // Arrange
        var item = new Item
        {
            Id = Guid.NewGuid(),
            Description = "Old Item",
            Date = DateTime.Now,
            Price = 100m,
            Weight = 1
        };
        await _itemRepository.Create(item);
        await _dbContext.SaveChangesAsync();

        // Act
        item.Description = "New Item";
        var result = await _itemRepository.Update(item);

        // Assert
        Assert.Equal("New Item", result.Description);
    }
}
