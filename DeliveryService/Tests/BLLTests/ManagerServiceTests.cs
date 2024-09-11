using AutoMapper;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Classes;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;
using NSubstitute;

namespace UnitTests.BLLTests;

public class ManagerServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ManagerService _managerService;
    private readonly IManagerRepository _managerRepository;
    private readonly IStorageRepository _storageRepository;

    public ManagerServiceTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _managerRepository = Substitute.For<IManagerRepository>();
        _storageRepository = Substitute.For<IStorageRepository>();

        _unitOfWork.ManagerRepository.Returns(_managerRepository);
        _unitOfWork.StorageRepository.Returns(_storageRepository);

        _managerService = new ManagerService(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldAddManager()
    {
        // Arrange
        var model = new SaveManagerModel
        {
            Name = "John Doe",
            StorageId = Guid.NewGuid()
        };

        var manager = new Manager { Id = Guid.NewGuid(), Name = model.Name };

        _mapper.Map<Manager>(model).Returns(manager);
        _storageRepository.Find(model.StorageId.Value).Returns(new Storage { Id = model.StorageId.Value });
        _managerRepository.Create(manager).Returns(Task.FromResult(manager));

        // Act
        var result = await _managerService.AddAsync(model);

        // Assert
        Assert.Equal(manager.Id, result);
        await _managerRepository.Received(1).Create(manager);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteManager()
    {
        // Arrange
        var managerId = Guid.NewGuid();

        // Act
        await _managerService.DeleteAsync(managerId);

        // Assert
        await _managerRepository.Received(1).Delete(managerId);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllManagers()
    {
        // Arrange
        var managers = new List<Manager>
        {
            new Manager { Id = Guid.NewGuid(), Name = "John Doe" },
            new Manager { Id = Guid.NewGuid(), Name = "Jane Doe" }
        };
        var managerReadModels = managers.Select(manager => new ManagerReadModel
        {
            Id = manager.Id,
            Name = manager.Name
        });

        _managerRepository.GetAll().Returns(managers.AsQueryable());
        _mapper.Map<IEnumerable<ManagerReadModel>>(Arg.Any<IEnumerable<Manager>>()).Returns(managerReadModels);

        // Act
        var result = await _managerService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnManagerById()
    {
        // Arrange
        var managerId = Guid.NewGuid();
        var manager = new Manager { Id = managerId, Name = "John Doe" };
        var managerReadModel = new ManagerReadModel { Id = managerId, Name = "John Doe" };

        _managerRepository.Find(managerId).Returns(Task.FromResult(manager));
        _mapper.Map<ManagerReadModel>(manager).Returns(managerReadModel);

        // Act
        var result = await _managerService.GetByIdAsync(managerId);

        // Assert
        Assert.Equal(managerReadModel.Id, result.Id);
        Assert.Equal(managerReadModel.Name, result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateManager()
    {
        // Arrange
        var managerId = Guid.NewGuid();
        var model = new UpdateManagerModel { Id = managerId, Name = "Updated Name", StorageId = Guid.NewGuid() };
        var manager = new Manager { Id = managerId, Name = "Original Name" };

        _managerRepository.Find(managerId).Returns(Task.FromResult(manager));
        _mapper.Map(model, manager).Returns(manager);
        _storageRepository.Find(model.StorageId.Value).Returns(new Storage { Id = model.StorageId.Value });
        _managerRepository.Update(manager).Returns(Task.FromResult(manager));

        // Act
        var result = await _managerService.UpdateAsync(model);

        // Assert
        Assert.Equal(manager.Id, result);
        await _managerRepository.Received(1).Update(manager);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}
