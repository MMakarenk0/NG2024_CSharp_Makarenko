using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
using DataLayer.Entities;

namespace BLL.Services.Classes;

public class StorageService : IStorageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StorageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Guid> AddAsync(SaveStorageModel model)
    {
        var storageRepository = _unitOfWork.StorageRepository;
        var managerRepository = _unitOfWork.ManagerRepository;
        var employeeRepository = _unitOfWork.EmployeeRepository;
        var itemRepository = _unitOfWork.ItemRepository;

        var storage = _mapper.Map<Storage>(model);

        if (model.DirectorId.HasValue)
        {
            var sender = await managerRepository.Find(model.DirectorId.Value);
            storage.Director = sender;
        }

        if (model.EmployeeIds != null && model.EmployeeIds.Any())
        {
            var employees = await employeeRepository.GetAllAsync(e => model.EmployeeIds.Contains(e.Id));
            storage.Employees = employees;
        }

        if (model.ItemIds != null && model.ItemIds.Any())
        {
            var items = await itemRepository.GetAllAsync(i => model.ItemIds.Contains(i.Id));
            storage.Items = items;
        }

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

        return _mapper.Map<IEnumerable<StorageReadModel>>(storageRepository.GetAll());
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

        await UpdateItems(model.ItemIds, storage);

        await UpdateEmployees(model.EmployeeIds, storage);

        await storageRepository.Update(storage);

        return storage.Id;
    }

    private async Task UpdateItems(List<Guid> updateItemsList, Storage storage)
    {
        var itemRepository = _unitOfWork.ItemRepository;
        var storageItemIds = storage.Items.Select(items => items.Id);

        var existingItems = await itemRepository.GetAllAsync(x => storageItemIds.Contains(x.Id));

        var itemsToAdd = updateItemsList.Except(existingItems.Select(item => item.Id)).ToList();

        var itemsToRemove = existingItems.Where(x => !updateItemsList.Contains(x.Id)).ToList();

        foreach (var item in itemsToAdd)
        {
            var itemToAdd = await itemRepository.Find(item);
            storage.Items.Add(itemToAdd);
        }

        foreach (var item in itemsToRemove)
        {
            storage.Items.Remove(item);
        }
    }
    private async Task UpdateEmployees(List<Guid> updateEmployeesList, Storage storage)
    {
        var employeeRepository = _unitOfWork.EmployeeRepository;
        var storageEmployeeIds = storage.Employees.Select(employee => employee.Id);

        var existingEmployees = await employeeRepository.GetAllAsync(x => storageEmployeeIds.Contains(x.Id));

        var employeesToAdd = updateEmployeesList.Except(existingEmployees.Select(employee => employee.Id)).ToList();

        var employeesToRemove = existingEmployees.Where(x => !updateEmployeesList.Contains(x.Id)).ToList();

        foreach (var employee in employeesToAdd)
        {
            var employeeToAdd = await employeeRepository.Find(employee);
            storage.Employees.Add(employeeToAdd);
        }

        foreach (var employee in employeesToRemove)
        {
            storage.Employees.Remove(employee);
        }
    }
}
