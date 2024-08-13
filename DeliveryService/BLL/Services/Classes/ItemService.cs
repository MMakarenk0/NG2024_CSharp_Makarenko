﻿using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;

namespace BLL.Services.Classes;

public class ItemService : IItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> AddAsync(SaveItemModel model)
    {
        var itemRepository = _unitOfWork.ItemRepository;
        var clientRepository = _unitOfWork.ClientRepository;
        var itemCategoryRepository = _unitOfWork.ItemCategoryRepository;
        var storageRepository = _unitOfWork.StorageRepository;

        var item = _mapper.Map<Item>(model);

        if (model.SenderId.HasValue)
        {
            var sender = await clientRepository.Find(model.SenderId.Value);
            item.Sender = sender;
        }

        if (model.ReceiverId.HasValue)
        {
            var receiver = await clientRepository.Find(model.ReceiverId.Value);
            item.Receiver = receiver;
        }

        if (model.StorageId.HasValue)
        {
            var storage = await storageRepository.Find(model.StorageId.Value);
            item.Storage = storage;
        }

        if (model.CategoryIds != null && model.CategoryIds.Any())
        {
            foreach (var categoryId in model.CategoryIds)
            {
                var itemCategory = new ItemCategory
                {
                    ItemId = item.Id,
                    CategoryId = categoryId
                };
                item.ItemCategories.Add(itemCategory);
            }
        }

        var result = await itemRepository.Create(item);

        await _unitOfWork.SaveChangesAsync();

        return result.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var itemRepository = _unitOfWork.ItemRepository;

        await itemRepository.Delete(id);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ItemReadModel>> GetAllAsync()
    {
        var itemRepository = _unitOfWork.ItemRepository;

        return _mapper.Map<IEnumerable<ItemReadModel>>(itemRepository.GetAll());
    }

    public async Task<ItemReadModel> GetByIdAsync(Guid id)
    {
        var itemRepository = _unitOfWork.ItemRepository;

        var item = await itemRepository.Find(id);

        return _mapper.Map<ItemReadModel>(item);
    }

    public async Task<IEnumerable<ItemReadModel>> GetBySenderPhone(string senderPhone)
    {
        var itemRepository = _unitOfWork.ItemRepository;
        var clientRepository = _unitOfWork.ClientRepository;

        var clients = clientRepository.GetAll();

        var client = clients.FirstOrDefault(c => c.Phone == senderPhone);

        if (client == null)
        {
            throw new Exception("Client not found");
        }

        var items = await itemRepository.GetAllAsync(i => i.SenderId == client.Id);

        return _mapper.Map<ICollection<ItemReadModel>>(items);
    }

    public async Task<IEnumerable<ItemReadModel>> GetByRecieverPhone(string receiverPhone)
    {
        var itemRepository = _unitOfWork.ItemRepository;
        var clientRepository = _unitOfWork.ClientRepository;

        var clients = clientRepository.GetAll();

        var client = clients.FirstOrDefault(c => c.Phone == receiverPhone);

        if (client == null)
        {
            throw new Exception("Client not found");
        }

        var items = await itemRepository.GetAllAsync(i => i.ReceiverId == client.Id);

        return _mapper.Map<ICollection<ItemReadModel>>(items);
    }

    public async Task<IEnumerable<ItemReadModel>> GetByStorage(Guid storageId)
    {
        var itemRepository = _unitOfWork.ItemRepository;

        var items = await itemRepository.GetAllAsync(i => i.StorageId == storageId && !i.isReceived);

        return _mapper.Map<IEnumerable<ItemReadModel>>(items);
    }

    public async Task<Guid> UpdateAsync(UpdateItemModel model)
    {
        var itemRepository = _unitOfWork.ItemRepository;
        var clientRepository = _unitOfWork.ClientRepository;
        var storageRepository = _unitOfWork.StorageRepository;
        var itemCategoryRepository = _unitOfWork.ItemCategoryRepository;

        var item = await itemRepository.Find(model.Id);

        if (item == null)
            throw new Exception($"Item with Id {model.Id} not found.");

        _mapper.Map(model, item);

        if (model.SenderId.HasValue)
        {
            var sender = await clientRepository.Find(model.SenderId.Value);
            item.Sender = sender;
        }

        if (model.ReceiverId.HasValue)
        {
            var receiver = await clientRepository.Find(model.ReceiverId.Value);
            item.Receiver = receiver;
        }

        if (model.StorageId.HasValue)
        {
            var storage = await storageRepository.Find(model.StorageId.Value);
            item.Storage = storage;
        }

        await UpdateItemCategory(model, itemCategoryRepository, item);

        var result = await itemRepository.Update(item);

        await _unitOfWork.SaveChangesAsync();

        return result.Id;
    }

    private async Task UpdateItemCategory(UpdateItemModel model, IItemCategoryRepository itemCategoryRepository, Item item)
    {
        if (model.CategoryIds != null && model.CategoryIds.Any())
        {
            var existingItemCategories = await itemCategoryRepository.GetAllAsync(ic => ic.ItemId == item.Id);
            foreach (var itemCategory in existingItemCategories)
            {
                await itemCategoryRepository.Delete(itemCategory.Id);
            }

            foreach (var categoryId in model.CategoryIds)
            {
                var itemCategory = new ItemCategory
                {
                    ItemId = item.Id,
                    CategoryId = categoryId
                };
                await itemCategoryRepository.Create(itemCategory);
            }
        }
    }
}