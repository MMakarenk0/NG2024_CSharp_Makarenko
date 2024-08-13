using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
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

        var category = _mapper.Map<Category>(model);

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
            throw new Exception($"Category with Id {model.Id} not found.");

        _mapper.Map(model, category);

        if (model.ItemIds != null && model.ItemIds.Any())
        {
            var currentItemCategories = category.ItemCategories.ToList();
            foreach (var itemCategory in currentItemCategories)
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
        await categoryRepository.Update(category);

        await _unitOfWork.SaveChangesAsync();

        return category.Id;
    }
}
