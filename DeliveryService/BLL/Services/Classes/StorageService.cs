using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;

namespace BLL.Services.Classes;

public class StorageService : IStorageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StorageService(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Guid> AddAsync(SaveStorageModel model)
    {
        var storageRepository = _unitOfWork.StorageRepository;

        var storage = _mapper.Map<Storage>(model);

        await storageRepository.Create(storage);

        await _unitOfWork.SaveChangesAsync();

        return storage.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var storageRepository = _unitOfWork.StorageRepository;

        await storageRepository.Delete(id);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<StorageReadModel>> GetAllAsync()
    {
        var storageRepository = _unitOfWork.StorageRepository;

        return _mapper.Map<IEnumerable<StorageReadModel>>(storageRepository);
    }

    public async Task<StorageReadModel> GetByIdAsync(Guid id)
    {
        var storageRepository = _unitOfWork.StorageRepository;

        var storage = await storageRepository.Find(id);

        return _mapper.Map<StorageReadModel>(storage);
    }

    public async Task<Guid> UpdateAsync(UpdateStorageModel model)
    {
        var storageRepository = _unitOfWork.StorageRepository;
        var managerRepository = _unitOfWork.ManagerRepository;
        var employeeRepository = _unitOfWork.EmployeeRepository;
        var itemRepository = _unitOfWork.ItemRepository;

        var storage = await storageRepository.Find(model.Id);
        if (storage == null)
            throw new Exception($"Storage with Id {model.Id} not found.");

        _mapper.Map(model, storage);

        if (model.DirectorId.HasValue)
        {
            var director = await managerRepository.Find(model.DirectorId.Value);
            if (director != null)
            {
                storage.Director = director;
            }
        }

        if (model.EmployeeIds != null && model.EmployeeIds.Any())
        {
            var employees = await employeeRepository.GetAllAsync(e => model.EmployeeIds.Contains(e.Id));
            storage.Employees = employees.ToList();
        }

        if (model.ItemIds != null && model.ItemIds.Any())
        {
            var items = await itemRepository.GetAllAsync(i => model.ItemIds.Contains(i.Id));
            storage.Items = items.ToList();
        }

        await storageRepository.Update(storage);
        return storage.Id;
    }
}
