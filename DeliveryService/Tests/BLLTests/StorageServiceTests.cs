using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Classes;
using DataLayer.Data.Infrastructure;
using DataLayer.Entities;
using NSubstitute;

namespace UnitTests.BLLTests;

public class StorageServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly StorageService _storageService;

    public StorageServiceTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _storageService = new StorageService(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldAddStorageAndReturnId()
    {
        // Arrange
        var model = new SaveStorageModel
        {
            Address = "123 Main St",
            Number = 1,
            DirectorId = Guid.NewGuid()
        };

        var storage = new Storage { Id = Guid.NewGuid() };
        var director = new Manager { Id = model.DirectorId.Value };

        _mapper.Map<Storage>(model).Returns(storage);
        _unitOfWork.ManagerRepository.Find(model.DirectorId.Value).Returns(Task.FromResult(director));
        _unitOfWork.StorageRepository.Create(storage).Returns(Task.FromResult(storage));

        // Act
        var result = await _storageService.AddAsync(model);

        // Assert
        Assert.Equal(storage.Id, result);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteStorage()
    {
        // Arrange
        var storageId = Guid.NewGuid();

        // Act
        await _storageService.DeleteAsync(storageId);

        // Assert
        await _unitOfWork.StorageRepository.Received(1).Delete(storageId);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStorages()
    {
        // Arrange
        var storages = new List<Storage>
        {
            new Storage { Id = Guid.NewGuid(), Address = "123 Main St", Number = 1 },
            new Storage { Id = Guid.NewGuid(), Address = "456 Elm St", Number = 2 }
        };
        var storageReadModels = storages.Select(storage => new StorageReadModel
        {
            Id = storage.Id,
            Address = storage.Address,
            Number = storage.Number
        });

        _unitOfWork.StorageRepository.GetAll().Returns(storages.AsQueryable());
        _mapper.Map<IEnumerable<StorageReadModel>>(Arg.Any<IEnumerable<Storage>>()).Returns(storageReadModels);

        // Act
        var result = await _storageService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnStorageById()
    {
        // Arrange
        var storageId = Guid.NewGuid();
        var storage = new Storage { Id = storageId, Address = "123 Main St", Number = 1 };

        _unitOfWork.StorageRepository.Find(storageId).Returns(Task.FromResult(storage));
        _mapper.Map<StorageReadModel>(storage).Returns(new StorageReadModel { Id = storageId });

        // Act
        var result = await _storageService.GetByIdAsync(storageId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(storageId, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStorageAndReturnId()
    {
        // Arrange
        var model = new UpdateStorageModel
        {
            Id = Guid.NewGuid(),
            Address = "123 Updated St",
            Number = 1,
            DirectorId = Guid.NewGuid()
        };

        var storage = new Storage { Id = model.Id };

        _unitOfWork.StorageRepository.Find(model.Id).Returns(Task.FromResult(storage));
        _mapper.Map(model, storage);
        _unitOfWork.StorageRepository.Update(storage).Returns(Task.FromResult(storage));
        _unitOfWork.ItemRepository.GetAllAsync(default).ReturnsForAnyArgs(new List<Item>());
        _unitOfWork.EmployeeRepository.GetAllAsync(default).ReturnsForAnyArgs(new List<Employee>());

        // Act
        var result = await _storageService.UpdateAsync(model);

        // Assert   
        Assert.Equal(model.Id, result);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}
