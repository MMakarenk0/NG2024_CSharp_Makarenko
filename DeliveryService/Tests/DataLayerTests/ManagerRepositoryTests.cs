using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.DataLayerTests;

public class ManagerRepositoryTests
{
    private readonly ManagerRepository _managerRepository;
    private readonly DeliveryServiceDbContext _dbContext;

    public ManagerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DeliveryServiceDbContext(options);

        _managerRepository = new ManagerRepository(_dbContext);
    }
    [Fact]
    public void GetAll_ShouldReturnAllManagers()
    {
        // Arrange
        var manager1 = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "Manager1"
        };
        var manager2 = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "Manager2"
        };
        _dbContext.Managers.AddRange(manager1, manager2);
        _dbContext.SaveChanges();

        // Act
        var result = _managerRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredManagers()
    {
        // Arrange
        var manager1 = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "Manager1"
        };
        var manager2 = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "Manager11"
        };
        var manager3 = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "Manager3"
        };
        _dbContext.Managers.AddRange(manager1, manager2, manager3);
        _dbContext.SaveChanges();

        // Act
        var result = await _managerRepository.GetAllAsync(c => c.Name.Contains("1"));

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Create_ShouldAddManagerToDbSet()
    {
        // Arrange
        var manager = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "New Manager"
        };

        // Act
        await _managerRepository.Create(manager);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdManager = await _dbContext.Managers.FindAsync(manager.Id);
        Assert.NotNull(createdManager);
        Assert.Equal("New Manager", createdManager.Name);
    }

    [Fact]
    public async Task Delete_ShouldDeleteManager_WhenManagerExists()
    {
        // Arrange
        var manager = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "New Manager"
        };
        await _managerRepository.Create(manager);

        // Act
        await _managerRepository.Delete(manager.Id);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdManager = await _dbContext.Managers.FindAsync(manager.Id);
        Assert.Null(createdManager);
    }

    [Fact]
    public async Task Find_ShouldReturnManager_WhenManagerExists()
    {
        // Arrange
        var manager = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "New Manager"
        };
        await _managerRepository.Create(manager);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _managerRepository.Find(manager.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Manager", result.Name);
    }

    [Fact]
    public async Task Update_ShouldModifyManager_WhenManagerExists()
    {
        // Arrange
        var manager = new Manager
        {
            Id = Guid.NewGuid(),
            Name = "Old Manager"
        };
        await _managerRepository.Create(manager);
        await _dbContext.SaveChangesAsync();

        // Act
        manager.Name = "New Manager";
        var result = await _managerRepository.Update(manager);

        // Assert
        Assert.Equal("New Manager", result.Name);
    }
}
