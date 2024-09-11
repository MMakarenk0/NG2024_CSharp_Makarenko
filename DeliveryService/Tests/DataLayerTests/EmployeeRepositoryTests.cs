using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.DataLayerTests;

public class EmployeeRepositoryTests
{
    private readonly EmployeeRepository _employeeRepository;
    private readonly DeliveryServiceDbContext _dbContext;

    public EmployeeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DeliveryServiceDbContext(options);

        _employeeRepository = new EmployeeRepository(_dbContext);
    }
    [Fact]
    public void GetAll_ShouldReturnAllEmployees()
    {
        // Arrange
        var employee1 = new Employee 
        { 
            Id = Guid.NewGuid(), 
            Name = "Employee1", 
            Phone = "1234567890", 
            Position = "Director", 
            Salary = 1 
        };
        var employee2 = new Employee 
        { 
            Id = Guid.NewGuid(), 
            Name = "Employee2",
            Phone = "0987654321", 
            Position = "Director",
            Salary = 1 
        };
        _dbContext.Employees.AddRange(employee1, employee2);
        _dbContext.SaveChanges();

        // Act
        var result = _employeeRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredEmployees()
    {
        // Arrange
        var employee1 = new Employee 
        { 
            Id = Guid.NewGuid(), 
            Name = "Employee1", 
            Phone = "1234567890", 
            Position = "Director", 
            Salary = 1 
        };
        var employee2 = new Employee 
        { 
            Id = Guid.NewGuid(), 
            Name = "Employee2", 
            Phone = "0987654321", 
            Position = "Director", 
            Salary = 1 
        };
        var employee3 = new Employee 
        { 
            Id = Guid.NewGuid(), 
            Name = "Employee3", 
            Phone = "0981234567", 
            Position = "Director", 
            Salary = 1 
        };
        _dbContext.Employees.AddRange(employee1, employee2, employee3);
        _dbContext.SaveChanges();

        // Act
        var result = await _employeeRepository.GetAllAsync(c => c.Phone.Contains("098"));

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Create_ShouldAddEmployeeToDbSet()
    {
        // Arrange
        var employee = new Employee 
        { 
            Id = Guid.NewGuid(), 
            Name = "New Client", 
            Phone = "1234567890", 
            Position = "Director", 
            Salary = 1 
        };

        // Act
        await _employeeRepository.Create(employee);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdEmployee = await _dbContext.Employees.FindAsync(employee.Id);
        Assert.NotNull(createdEmployee);
        Assert.Equal("New Client", createdEmployee.Name);
    }

    [Fact]
    public async Task Delete_ShouldDeleteEmployee_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new Employee {
            Id = employeeId, 
            Name = "New Client", 
            Phone = "1234567890", 
            Position = "Director", 
            Salary = 1 
        };
        await _employeeRepository.Create(employee);

        // Act
        await _employeeRepository.Delete(employeeId);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdClient = await _dbContext.Employees.FindAsync(employeeId);
        Assert.Null(createdClient);
    }

    [Fact]
    public async Task Find_ShouldReturnEmployee_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new Employee 
        { 
            Id = employeeId, 
            Name = "New Client", 
            Phone = "1234567890", 
            Position = "Director", 
            Salary = 1 
        };
        await _employeeRepository.Create(employee);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _employeeRepository.Find(employeeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Client", result.Name);
    }

    [Fact]
    public async Task Update_ShouldModifyEmployee_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new Employee 
        { 
            Id = employeeId, 
            Name = "Old Name", 
            Phone = "1234567890", 
            Position = "Director", 
            Salary = 1 
        };
        await _employeeRepository.Create(employee);
        await _dbContext.SaveChangesAsync();

        // Act
        employee.Name = "New Name";
        var result = await _employeeRepository.Update(employee);

        // Assert
        Assert.Equal("New Name", result.Name);
    }
}
