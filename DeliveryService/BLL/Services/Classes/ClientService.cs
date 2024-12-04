using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
using DataLayer.Entities;

namespace BLL.Services.Classes;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClientService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Guid> AddAsync(SaveClientModel model)
    {
        var clientRepository = _unitOfWork.ClientRepository;

        var client = _mapper.Map<Client>(model);

        var result = await clientRepository.Create(client);

        await _unitOfWork.SaveChangesAsync();

        return result.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var clientRepository = _unitOfWork.ClientRepository;

        await clientRepository.Delete(id);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ClientReadModel>> GetAllAsync()
    {
        var clientRepository = _unitOfWork.ClientRepository;

        return _mapper.Map<IEnumerable<ClientReadModel>>(clientRepository.GetAll());
    }

    public async Task<ClientReadModel> GetByIdAsync(Guid id)
    {
        var clientRepository = _unitOfWork.ClientRepository;

        var client = await clientRepository.Find(id);

        return _mapper.Map<ClientReadModel>(client);
    }

    public async Task<Guid> UpdateAsync(UpdateClientModel model)
    {
        var clientRepository = _unitOfWork.ClientRepository;

        var client = await clientRepository.Find(model.Id);
        if (client == null)
            throw new Exception($"Client with Id {model.Id} not found.");

        _mapper.Map(model, client);

        await clientRepository.Update(client);
        return client.Id;
    }
}
