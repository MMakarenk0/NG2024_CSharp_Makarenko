using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.DataLayerTests;

public class ClientRepositoryTests
{
    private readonly ClientRepository _clientRepository;
    private readonly DeliveryServiceDbContext _dbContext;

    public ClientRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DeliveryServiceDbContext(options);

        _clientRepository = new ClientRepository(_dbContext);
    }
    [Fact]
    public void GetAll_ShouldReturnAllClients()
    {
        // Arrange
        var client1 = new Client 
        { 
            Id = Guid.NewGuid(),
            Name = "Client 1",
            Phone = "1234567890" 
        };
        var client2 = new Client 
        { 
            Id = Guid.NewGuid(), 
            Name = "Client 2",
            Phone = "0987654321" 
        };
        _dbContext.Clients.AddRange(client1, client2);
        _dbContext.SaveChanges();

        // Act
        var result = _clientRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredClients()
    {
        // Arrange
        var client1 = new Client
        {
            Id = Guid.NewGuid(),
            Name = "Client 1", 
            Phone = "1234567890"
        };
        var client2 = new Client 
        { 
            Id = Guid.NewGuid(),
            Name = "Client 2",
            Phone = "0987654321"
        };
        var client3 = new Client 
        { 
            Id = Guid.NewGuid(),
            Name = "Client 3",
            Phone = "0981234567" 
        };
        _dbContext.Clients.AddRange(client1, client2, client3);
        _dbContext.SaveChanges();

        // Act
        var result =  await _clientRepository.GetAllAsync(c => c.Phone.Contains("098"));

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Create_ShouldAddClientToDbSet()
    {
        // Arrange
        var client = new Client 
        { 
            Id = Guid.NewGuid(),
            Name = "New Client",
            Phone = "1234567890" 
        };

        // Act
        await _clientRepository.Create(client);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdClient = await _dbContext.Clients.FindAsync(client.Id);
        Assert.NotNull(createdClient);
        Assert.Equal("New Client", createdClient.Name);
    }

    [Fact]
    public async Task Delete_ShouldDeleteClient_WhenClientExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client 
        { 
            Id = clientId,
            Name = "New Client",
            Phone = "1234567890"
        };
        await _clientRepository.Create(client);

        // Act
        await _clientRepository.Delete(clientId);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdClient = await _dbContext.Clients.FindAsync(clientId);
        Assert.Null(createdClient);
    }

    [Fact]
    public async Task Find_ShouldReturnClient_WhenClientExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client 
        { 
            Id = clientId,
            Name = "New Client",
            Phone = "1234567890"
        };
        await _clientRepository.Create(client);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _clientRepository.Find(clientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Client", result.Name);
    }

    [Fact]
    public async Task Update_ShouldModifyClient_WhenClientExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client 
        { 
            Id = clientId,
            Name = "Old Name",
            Phone = "1234567890" 
        };
        await _clientRepository.Create(client);
        await _dbContext.SaveChangesAsync();

        // Act
        client.Name = "New Name";
        var result = await _clientRepository.Update(client);

        // Assert
        Assert.Equal("New Name", result.Name);
    }
}
