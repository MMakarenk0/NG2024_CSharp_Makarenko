using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.DataLayerTests;

public class StorageRepositoryTests
{
    private readonly StorageRepository _storageRepository;
    private readonly DeliveryServiceDbContext _dbContext;

    public StorageRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DeliveryServiceDbContext(options);

        _storageRepository = new StorageRepository(_dbContext);
    }
    [Fact]
    public void GetAll_ShouldReturnAllStorages()
    {
        // Arrange
        var storage1 = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 1,
        };
        var storage2 = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 2,
        };
        _dbContext.Storages.AddRange(storage1, storage2);
        _dbContext.SaveChanges();

        // Act
        var result = _storageRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredStorages()
    {
        // Arrange
        var storage1 = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 1,
        };
        var storage2 = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 2,
        };
        var storage3 = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 3,
        };
        _dbContext.Storages.AddRange(storage1, storage2, storage3);
        _dbContext.SaveChanges();

        // Act
        var result = await _storageRepository.GetAllAsync(c => c.Number > 1);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Create_ShouldAddStorageToDbSet()
    {
        // Arrange
        var storage = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 1,
        };

        // Act
        await _storageRepository.Create(storage);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdStorage = await _dbContext.Storages.FindAsync(storage.Id);
        Assert.NotNull(createdStorage);
        Assert.Equal(1, createdStorage.Number);
    }

    [Fact]
    public async Task Delete_ShouldDeleteStorage_WhenStorageExists()
    {
        // Arrange
        var storage = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 1,
        };
        await _storageRepository.Create(storage);

        // Act
        await _storageRepository.Delete(storage.Id);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdStorage = await _dbContext.Storages.FindAsync(storage.Id);
        Assert.Null(createdStorage);
    }

    [Fact]
    public async Task Find_ShouldReturnStorage_WhenStorageExists()
    {
        // Arrange
        var storage = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 1,
        };
        await _storageRepository.Create(storage);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _storageRepository.Find(storage.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Number);
    }

    [Fact]
    public async Task Update_ShouldModifyStorage_WhenStorageExists()
    {
        // Arrange
        var storage = new Storage
        {
            Id = Guid.NewGuid(),
            Address = "Adress",
            Number = 1,
        };
        await _storageRepository.Create(storage);
        await _dbContext.SaveChangesAsync();

        // Act
        storage.Number = 2;
        var result = await _storageRepository.Update(storage);

        // Assert
        Assert.Equal(2, result.Number);
    }
}
