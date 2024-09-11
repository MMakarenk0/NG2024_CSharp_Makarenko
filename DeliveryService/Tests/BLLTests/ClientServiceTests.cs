using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Classes;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;
using NSubstitute;

namespace UnitTests.BLLTests;

public class ClientServiceTests
{
    private readonly ClientService _clientService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClientRepository _clientRepository;

    public ClientServiceTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _clientRepository = Substitute.For<IClientRepository>();

        _unitOfWork.ClientRepository.Returns(_clientRepository);

        _clientService = new ClientService(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldAddClientAndReturnId()
    {
        // Arrange
        var saveModel = new SaveClientModel { Name = "Test Client", Phone = "1234567890" };
        var client = new Client { Id = Guid.NewGuid(), Name = "Test Client", Phone = "1234567890" };

        _mapper.Map<Client>(saveModel).Returns(client);
        _clientRepository.Create(client).Returns(Task.FromResult(client));

        // Act
        var result = await _clientService.AddAsync(saveModel);

        // Assert
        Assert.Equal(client.Id, result);
        await _clientRepository.Received(1).Create(client);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        await _clientService.DeleteAsync(clientId);

        // Assert
        await _clientRepository.Received(1).Delete(clientId);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllClients()
    {
        // Arrange
        var clients = new List<Client>
    {
        new Client { Id = Guid.NewGuid(), Name = "Client 1", Phone = "1234567890" },
        new Client { Id = Guid.NewGuid(), Name = "Client 2", Phone = "0987654321" }
    };

        _clientRepository.GetAll().Returns(clients.AsQueryable());

        var clientReadModels = clients.Select(client => new ClientReadModel
        {
            Id = client.Id,
            Name = client.Name,
            Phone = client.Phone
        });

        _mapper.Map<IEnumerable<ClientReadModel>>(Arg.Any<IEnumerable<Client>>())
               .Returns(clientReadModels);

        // Act
        var result = await _clientService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(clients[0].Id, result.First().Id);
        Assert.Equal(clients[1].Id, result.Last().Id);
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client { Id = clientId, Name = "Test Client", Phone = "1234567890" };

        _clientRepository.Find(clientId).Returns(Task.FromResult(client));
        _mapper.Map<ClientReadModel>(client).Returns(new ClientReadModel { Id = clientId, Name = "Test Client", Phone = "1234567890" });

        // Act
        var result = await _clientService.GetByIdAsync(clientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientId, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateClient()
    {
        // Arrange
        var updateModel = new UpdateClientModel { Id = Guid.NewGuid(), Name = "Updated Client", Phone = "0987654321" };
        var client = new Client { Id = updateModel.Id, Name = "Original Client", Phone = "1234567890" };

        _clientRepository.Find(updateModel.Id).Returns(Task.FromResult(client));

        // Act
        var result = await _clientService.UpdateAsync(updateModel);

        // Assert
        Assert.Equal(client.Id, result);
        await _clientRepository.Received(1).Update(client);
    }
}