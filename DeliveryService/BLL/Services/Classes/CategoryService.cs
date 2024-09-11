using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;

namespace BLL.Services.Classes;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Guid> AddAsync(SaveCategoryModel model)
    {
        var categoryRepository = _unitOfWork.CategoryRepository;
        var itemCategoryRepository = _unitOfWork.ItemCategoryRepository;

        var category = _mapper.Map<Category>(model);

        if (model.ItemIds != null && model.ItemIds.Any())
        {
            foreach (var itemId in model.ItemIds)
            {
                var itemCategory = new ItemCategory
                {
                    ItemId = itemId,
                    CategoryId = category.Id
                };
                category.ItemCategories.Add(itemCategory);
                await itemCategoryRepository.Create(itemCategory);
            }
        }

        var result = await categoryRepository.Create(category);

        await _unitOfWork.SaveChangesAsync();

        return result.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var categoryRepository = _unitOfWork.CategoryRepository;

        await categoryRepository.Delete(id);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryReadModel>> GetAllAsync()
    {
        var categoryRepository = _unitOfWork.CategoryRepository;

        return _mapper.Map<IEnumerable<CategoryReadModel>>(categoryRepository.GetAll());
    }

    public async Task<CategoryReadModel> GetByIdAsync(Guid id)
    {
        var categoryRepository = _unitOfWork.CategoryRepository;

        var category = await categoryRepository.Find(id);

        return _mapper.Map<CategoryReadModel>(category);
    }

    public async Task<Guid> UpdateAsync(UpdateCategoryModel model)
    {
        var categoryRepository = _unitOfWork.CategoryRepository;
        var itemCategoryRepository = _unitOfWork.ItemCategoryRepository;

        var category = await categoryRepository.Find(model.Id);
        if (category == null)
            throw new KeyNotFoundException($"Category with Id {model.Id} not found.");

        _mapper.Map(model, category);

        await UpdateItemCategory(model, itemCategoryRepository, category);

        await categoryRepository.Update(category);

        await _unitOfWork.SaveChangesAsync();

        return category.Id;
    }

    private async Task UpdateItemCategory(UpdateCategoryModel model, IItemCategoryRepository itemCategoryRepository, Category category)
    {
        if (model.ItemIds != null && model.ItemIds.Any())
        {
            var existingItemCategories = category.ItemCategories.ToList();
            foreach (var itemCategory in existingItemCategories)
            {
                await itemCategoryRepository.Delete(itemCategory.Id);
            }

            foreach (var itemId in model.ItemIds)
            {
                var itemCategory = new ItemCategory
                {
                    CategoryId = category.Id,
                    ItemId = itemId
                };
                await itemCategoryRepository.Create(itemCategory);
            }
        }
    }
}
