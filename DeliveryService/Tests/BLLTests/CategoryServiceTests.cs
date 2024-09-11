using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Classes;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;
using NSubstitute;

namespace UnitTests.BLLTests
{
    public class CategoryServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IItemCategoryRepository _itemCategoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _categoryRepository = Substitute.For<ICategoryRepository>();
            _itemCategoryRepository = Substitute.For<IItemCategoryRepository>();
            _mapper = Substitute.For<IMapper>();

            _unitOfWork.CategoryRepository.Returns(_categoryRepository);
            _unitOfWork.ItemCategoryRepository.Returns(_itemCategoryRepository);

            _categoryService = new CategoryService(_unitOfWork, _mapper);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateCategoryAndItemCategories()
        {
            // Arrange
            var saveModel = new SaveCategoryModel
            {
                Description = "Test Category",
                ItemIds = new List<Guid> { Guid.NewGuid() }
            };
            var category = new Category { Id = Guid.NewGuid(), Description = "Test Category" };

            _mapper.Map<Category>(saveModel).Returns(category);
            _categoryRepository.Create(Arg.Any<Category>()).Returns(category);

            // Act
            var result = await _categoryService.AddAsync(saveModel);

            // Assert
            await _categoryRepository.Received(1).Create(Arg.Any<Category>());
            await _itemCategoryRepository.Received(1).Create(Arg.Any<ItemCategory>());
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            // Act
            await _categoryService.DeleteAsync(categoryId);

            // Assert
            await _categoryRepository.Received(1).Delete(categoryId);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Description = "Test Category" } };
            _categoryRepository.GetAll().Returns(categories.AsQueryable());

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            _mapper.Received(1).Map<IEnumerable<CategoryReadModel>>(Arg.Any<IQueryable<Category>>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCategoryAndItemCategories()
        {
            // Arrange
            var updateModel = new UpdateCategoryModel
            {
                Id = Guid.NewGuid(),
                Description = "Updated Category",
                ItemIds = new List<Guid> { Guid.NewGuid() }
            };
            var itemCategory = new ItemCategory { CategoryId = updateModel.Id, ItemId = Guid.NewGuid() };
            var category = new Category
            {
                Id = updateModel.Id,
                Description = "Original Category",
                ItemCategories = new List<ItemCategory> { itemCategory }
            };

            _categoryRepository.Find(updateModel.Id).Returns(Task.FromResult(category));

            // Act
            var result = await _categoryService.UpdateAsync(updateModel);

            // Assert
            await _categoryRepository.Received(1).Update(category);
            await _itemCategoryRepository.Received(1).Delete(itemCategory.Id);
            await _itemCategoryRepository.Received(1).Create(Arg.Any<ItemCategory>());
            await _unitOfWork.Received(1).SaveChangesAsync();
        }
    }
}
