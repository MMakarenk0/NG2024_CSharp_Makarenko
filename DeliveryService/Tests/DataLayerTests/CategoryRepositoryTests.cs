using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.DataLayerTests;

public class CategoryRepositoryTests
{
    private readonly CategoryRepository _categoryRepository;
    private readonly DeliveryServiceDbContext _dbContext;

    public CategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DeliveryServiceDbContext(options);

        _categoryRepository = new CategoryRepository(_dbContext);
    }


    [Fact]
    public async Task Create_ShouldAddCategoryToDbSet()
    {
        // Arrange
        var category = new Category 
        { 
            Id = Guid.NewGuid(), 
            Description = "Test Category" 
        };

        // Act
        await _categoryRepository.Create(category);

        // Assert
        var createdCategory = await _dbContext.Categories.FindAsync(category.Id);
        Assert.NotNull(createdCategory);
        Assert.Equal("Test Category", createdCategory.Description);
    }

    [Fact]
    public async Task Find_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var expectedCategory = new Category 
        { 
            Id = categoryId, 
            Description = "Test Category" 
        };

        _dbContext.Categories.Add(expectedCategory);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _categoryRepository.Find(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Category", result.Description);
    }

    [Fact]
    public async Task Delete_ShouldRemoveCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var expectedCategory = new Category 
        {
            Id = categoryId, 
            Description = "Test Category" 
        };

        _dbContext.Categories.Add(expectedCategory);
        await _dbContext.SaveChangesAsync();

        // Act
        await _categoryRepository.Delete(categoryId);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdCategory = await _dbContext.Categories.FindAsync(categoryId);
        Assert.Null(createdCategory);
    }

    [Fact]
    public void GetAll_ShouldReturnAllCategories()
    {
        // Arrange
        var category1 = new Category 
        { 
            Id = Guid.NewGuid(),
            Description = "Category 1"
        };
        var category2 = new Category 
        { 
            Id = Guid.NewGuid(),
            Description = "Category 2" 
        };
        _dbContext.Categories.AddRange(category1, category2);
        _dbContext.SaveChanges();

        // Act
        var result = _categoryRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredCategories()
    {
        // Arrange
        var category1 = new Category 
        { 
            Id = Guid.NewGuid(),
            Description = "Category 1" 
        };
        var category2 = new Category 
        { 
            Id = Guid.NewGuid(),
            Description = "Category 2"
        };
        _dbContext.Categories.AddRange(category1, category2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _categoryRepository.GetAllAsync(c => c.Description.Contains("1"));

        // Assert
        Assert.Single(result);
        Assert.Equal("Category 1", result.First().Description);
    }

    [Fact]
    public async Task Update_ShouldModifyCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category { 
            Id = Guid.NewGuid(),
            Description = "Old Description"
        };
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        // Act
        category.Description = "New Description";
        await _categoryRepository.Update(category);

        // Assert
        var updatedCategory = await _dbContext.Categories.FindAsync(category.Id);
        Assert.NotNull(updatedCategory);
        Assert.Equal("New Description", updatedCategory.Description);
    }
}
