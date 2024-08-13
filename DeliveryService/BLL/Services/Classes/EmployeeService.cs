using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Realization;
using DataLayer.Entities;

namespace BLL.Services.Classes;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Guid> AddAsync(SaveEmployeeModel model)
    {
        var employeeRepository = _unitOfWork.EmployeeRepository;

        var employee = _mapper.Map<Employee>(model);

        var result = await employeeRepository.Create(employee);

        await _unitOfWork.SaveChangesAsync();

        return result.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var employeeRepository = _unitOfWork.EmployeeRepository;

        await employeeRepository.Delete(id);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<EmployeeReadModel>> GetAllAsync()
    {
        var employeeRepository = _unitOfWork.EmployeeRepository;

        return _mapper.Map<IEnumerable<EmployeeReadModel>>(employeeRepository.GetAll());
    }

    public async Task<EmployeeReadModel> GetByIdAsync(Guid id)
    {
        var employeeRepository = _unitOfWork.EmployeeRepository;

        var employee = await employeeRepository.Find(id);

        return _mapper.Map<EmployeeReadModel>(employee);
    }

    public async Task<Guid> UpdateAsync(UpdateEmployeeModel model)
    {
        var employeeRepository = _unitOfWork.EmployeeRepository;
        var storageRepository = _unitOfWork.StorageRepository;

        var employee = await employeeRepository.Find(model.Id);
        if (employee == null)
            throw new Exception($"Employee with Id {model.Id} not found.");

        _mapper.Map(model, employee);

        if (model.StorageId.HasValue)
        {
            var storage = await storageRepository.Find(model.StorageId.Value);
            if (storage != null)
            {
                employee.Storage = storage;
            }
        }

        await employeeRepository.Update(employee);

        await _unitOfWork.SaveChangesAsync();

        return employee.Id;
    }
}