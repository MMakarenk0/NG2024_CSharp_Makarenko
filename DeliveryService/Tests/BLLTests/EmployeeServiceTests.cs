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

public class EmployeeServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IStorageRepository _storageRepository;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _employeeRepository = Substitute.For<IEmployeeRepository>();
        _storageRepository = Substitute.For<IStorageRepository>();

        _unitOfWork.EmployeeRepository.Returns(_employeeRepository);
        _unitOfWork.StorageRepository.Returns(_storageRepository);

        _employeeService = new EmployeeService(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEmployeeAndReturnId()
    {
        // Arrange
        var saveModel = new SaveEmployeeModel
        {
            Name = "John Doe",
            StorageId = Guid.NewGuid()
        };
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "John Doe"
        };
        var storage = new Storage { Id = saveModel.StorageId.Value };

        _mapper.Map<Employee>(saveModel).Returns(employee);
        _storageRepository.Find(saveModel.StorageId.Value).Returns(Task.FromResult(storage));
        _employeeRepository.Create(employee).Returns(Task.FromResult(employee));

        // Act
        var result = await _employeeService.AddAsync(saveModel);

        // Assert
        Assert.Equal(employee.Id, result);
        await _storageRepository.Received(1).Find(saveModel.StorageId.Value);
        await _employeeRepository.Received(1).Create(employee);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEmployee()
    {
        // Arrange
        var employeeId = Guid.NewGuid();

        // Act
        await _employeeService.DeleteAsync(employeeId);

        // Assert
        await _employeeRepository.Received(1).Delete(employeeId);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEmployees()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Employee 1"
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Employee 2"
            }
        };

        _employeeRepository.GetAll().Returns(employees.AsQueryable());

        var employeeReadModels = employees.Select(employee => new EmployeeReadModel
        {
            Id = employee.Id,
            Name = employee.Name,
        });

        _mapper.Map<IEnumerable<EmployeeReadModel>>(Arg.Any<IEnumerable<Employee>>())
            .Returns(employeeReadModels);

        // Act
        var result = await _employeeService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(employees[0].Id, result.First().Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployeeById()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new Employee
        {
            Id = employeeId,
            Name = "John Doe"
        };
        var employeeReadModel = new EmployeeReadModel
        {
            Id = employeeId,
            Name = "John Doe"
        };

        _employeeRepository.Find(employeeId).Returns(Task.FromResult(employee));
        _mapper.Map<EmployeeReadModel>(employee).Returns(employeeReadModel);

        // Act
        var result = await _employeeService.GetByIdAsync(employeeId);

        // Assert
        Assert.Equal(employeeReadModel.Id, result.Id);
    }

    [Fact]
    public async Task GetByStorage_ShouldReturnEmployeesByStorageId()
    {
        // Arrange
        var storageId = Guid.NewGuid();
        var employees = new List<Employee>
        {
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Employee 1"
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Employee 2"
            }
        };
        var storage = new Storage
        {
            Id = storageId,
            Employees = employees
        };

        _storageRepository.Find(storageId).Returns(Task.FromResult(storage));
        _mapper.Map<IEnumerable<EmployeeReadModel>>(employees).Returns(employees.Select(e => new EmployeeReadModel
        {
            Id = e.Id,
            Name = e.Name
        }));

        // Act
        var result = await _employeeService.GetByStorage(storageId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmployeeAndReturnId()
    {
        // Arrange
        var updateModel = new UpdateEmployeeModel
        {
            Id = Guid.NewGuid(),
            Name = "Updated Employee",
            StorageId = Guid.NewGuid()
        };
        var employee = new Employee
        {
            Id = updateModel.Id,
            Name = "Original Employee"
        };
        var storage = new Storage { Id = updateModel.StorageId.Value };

        _employeeRepository.Find(updateModel.Id).Returns(Task.FromResult(employee));
        _storageRepository.Find(updateModel.StorageId.Value).Returns(Task.FromResult(storage));

        // Act
        var result = await _employeeService.UpdateAsync(updateModel);

        // Assert
        Assert.Equal(employee.Id, result);
        await _employeeRepository.Received(1).Update(employee);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}
