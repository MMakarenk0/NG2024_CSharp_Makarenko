using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.DataLayerTests;

public class ItemCategoryRepositoryTests
{
    private readonly ItemCategoryRepository _itemCategoryRepository;
    private readonly DeliveryServiceDbContext _dbContext;

    public ItemCategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DeliveryServiceDbContext(options);

        _itemCategoryRepository = new ItemCategoryRepository(_dbContext);
    }

    [Fact]
    public void GetAll_ShouldReturnAllItemCategories()
    {
        // Arrange
        var itemId1 = Guid.NewGuid();
        var categoryId1 = Guid.NewGuid();
        var itemCategory1 = new ItemCategory
        {
            ItemId = itemId1,
            Item = new Item
            {
                Id = itemId1,
                Description = "Item1",
                Date = DateTime.Now,
                Price = 100m,
                Weight = 1
            },
            CategoryId = categoryId1,
            Category = new Category
            {
                Id = categoryId1,
                Description = "Test Category1"
            }
        };

        var itemId2 = Guid.NewGuid();
        var categoryId2 = Guid.NewGuid();
        var itemCategory2 = new ItemCategory
        {
            ItemId = itemId2,
            Item = new Item
            {
                Id = itemId2,
                Description = "Item2",
                Date = DateTime.Now,
                Price = 100m,
                Weight = 1
            },
            CategoryId = categoryId2,
            Category = new Category
            {
                Id = categoryId2,
                Description = "Test Category2"
            }
        };
        _dbContext.ItemCategories.AddRange(itemCategory1, itemCategory2);
        _dbContext.SaveChanges();

        // Act
        var result = _itemCategoryRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredItemCategories()
    {
        // Arrange
        var itemId1 = Guid.NewGuid();
        var categoryId1 = Guid.NewGuid();
        var itemCategory1 = new ItemCategory
        {
            ItemId = itemId1,
            Item = new Item
            {
                Id = itemId1,
                Description = "Item1",
                Date = DateTime.Now,
                Price = 100m,
                Weight = 1
            },
            CategoryId = categoryId1,
            Category = new Category
            {
                Id = categoryId1,
                Description = "Test Category1"
            }
        };

        var itemId2 = Guid.NewGuid();
        var categoryId2 = Guid.NewGuid();
        var itemCategory2 = new ItemCategory
        {
            ItemId = itemId2,
            Item = new Item
            {
                Id = itemId2,
                Description = "Item2",
                Date = DateTime.Now,
                Price = 100m,
                Weight = 1
            },
            CategoryId = categoryId2,
            Category = new Category
            {
                Id = categoryId2,
                Description = "Test Category2"
            }
        };
        _dbContext.ItemCategories.AddRange(itemCategory1, itemCategory2);
        _dbContext.SaveChanges();

        // Act
        var result = await _itemCategoryRepository.GetAllAsync(ic => ic.Category.Description.Contains("2"));

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task Create_ShouldAddItemCategoryToDbSet()
    {
        // Arrange
        var itemId1 = Guid.NewGuid();
        var categoryId1 = Guid.NewGuid();
        var itemCategory1 = new ItemCategory
        {
            ItemId = itemId1,
            Item = new Item
            {
                Id = itemId1,
                Description = "Item1",
                Date = DateTime.Now,
                Price = 100m,
                Weight = 1
            },
            CategoryId = categoryId1,
            Category = new Category
            {
                Id = categoryId1,
                Description = "Test Category1"
            }
        };

        // Act
        await _itemCategoryRepository.Create(itemCategory1);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdItemCategory = _itemCategoryRepository.GetAll().FirstOrDefault();
        Assert.NotNull(createdItemCategory);
        Assert.Equal("Item1", createdItemCategory.Item.Description);
    }

    [Fact]
    public async Task Delete_ShouldDeleteItemCategory_WhenItemCategoryExists()
    {
        // Arrange
        var itemId1 = Guid.NewGuid();
        var categoryId1 = Guid.NewGuid();
        var itemCategory1 = new ItemCategory
        {
            ItemId = itemId1,
            Item = new Item
            {
                Id = itemId1,
                Description = "Item1",
                Date = DateTime.Now,
                Price = 100m,
                Weight = 1
            },
            CategoryId = categoryId1,
            Category = new Category
            {
                Id = categoryId1,
                Description = "Test Category1"
            }
        };
        await _itemCategoryRepository.Create(itemCategory1);

        // Act
        await _itemCategoryRepository.Delete(itemId1, categoryId1);
        await _dbContext.SaveChangesAsync();

        // Assert
        var createdItem = _itemCategoryRepository.GetAll().FirstOrDefault();
        Assert.Null(createdItem);
    }

    [Fact]
    public async Task Find_ShouldReturnItemCategory_WhenItemCategoryExists()
    {
        // Arrange
        var itemId1 = Guid.NewGuid();
        var categoryId1 = Guid.NewGuid();
        var itemCategory1 = new ItemCategory
        {
            ItemId = itemId1,
            Item = new Item
            {
                Id = itemId1,
                Description = "Item1",
                Date = DateTime.Now,
                Price = 100m,
                Weight = 1
            },
            CategoryId = categoryId1,
            Category = new Category
            {
                Id = categoryId1,
                Description = "Test Category1"
            }
        };
        await _itemCategoryRepository.Create(itemCategory1);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _itemCategoryRepository.Find(itemId1, categoryId1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Item1", result.Item.Description);
    }

    [Fact]
    public async Task Update_ShouldModifyItemCategory_WhenItemCategoryExists()
    {
        // Arrange
        var itemId1 = Guid.NewGuid();
        var categoryId1 = Guid.NewGuid();
        var itemCategory1 = new ItemCategory
        {
            Id = Guid.NewGuid(),
            ItemId = itemId1,
            Item = new Item
            {
                Id = itemId1,
                Description = "Old Item",
                Date = DateTime.Now,
                Price = 100m,
                Weight = 1
            },
            CategoryId = categoryId1,
            Category = new Category
            {
                Id = categoryId1,
                Description = "Old Test Category"
            }
        };
        await _itemCategoryRepository.Create(itemCategory1);
        await _dbContext.SaveChangesAsync();

        // Act
        itemCategory1.Item.Description = "New Item";
        itemCategory1.Category.Description = "New Category";
        var result = await _itemCategoryRepository.Update(itemCategory1);

        // Assert
        Assert.Equal("New Item", result.Item.Description);
        Assert.Equal("New Category", result.Category.Description);
    }
}
