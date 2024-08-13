using AutoMapper;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
using DataLayer.Entities;

namespace BLL.Services.Classes;

public class ManagerService : IManagerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManagerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Guid> AddAsync(SaveManagerModel model)
    {
        var managerRepository = _unitOfWork.ManagerRepository;

        var manager = _mapper.Map<Manager>(model);

        var result = await managerRepository.Create(manager);

        await _unitOfWork.SaveChangesAsync();

        return result.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var managerRepository = _unitOfWork.ManagerRepository;

        await managerRepository.Delete(id);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ManagerReadModel>> GetAllAsync()
    {
        var managerRepository = _unitOfWork.ManagerRepository;

        return _mapper.Map<IEnumerable<ManagerReadModel>>(managerRepository.GetAll());
    }

    public async Task<ManagerReadModel> GetByIdAsync(Guid id)
    {
        var managerRepository = _unitOfWork.ManagerRepository;

        var manager = await managerRepository.Find(id);

        return _mapper.Map<ManagerReadModel>(manager);
    }

    public async Task<Guid> UpdateAsync(UpdateManagerModel model)
    {
        throw new NotImplementedException();
    }
}