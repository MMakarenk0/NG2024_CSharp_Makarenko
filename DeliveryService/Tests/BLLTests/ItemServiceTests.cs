using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Classes;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;
using NSubstitute;

namespace UnitTests.BLLTests;

public class ItemServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IItemRepository _itemRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IStorageRepository _storageRepository;
    private readonly ItemService _itemService;

    public ItemServiceTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _itemRepository = Substitute.For<IItemRepository>();
        _clientRepository = Substitute.For<IClientRepository>();
        _itemCategoryRepository = Substitute.For<IItemCategoryRepository>();
        _storageRepository = Substitute.For<IStorageRepository>();

        _unitOfWork.ItemRepository.Returns(_itemRepository);
        _unitOfWork.ClientRepository.Returns(_clientRepository);
        _unitOfWork.ItemCategoryRepository.Returns(_itemCategoryRepository);
        _unitOfWork.StorageRepository.Returns(_storageRepository);

        _itemService = new ItemService(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldAddItemAndReturnId()
    {
        // Arrange
        var saveModel = new SaveItemModel
        {
            Description = "Test Item",
            SenderId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            StorageId = Guid.NewGuid(),
            CategoryIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };
        var item = new Item { Id = Guid.NewGuid(), Description = "Test Item" };
        var sender = new Client { Id = saveModel.SenderId.Value };
        var receiver = new Client { Id = saveModel.ReceiverId.Value };
        var storage = new Storage { Id = saveModel.StorageId.Value };

        _mapper.Map<Item>(saveModel).Returns(item);
        _clientRepository.Find(saveModel.SenderId.Value).Returns(Task.FromResult(sender));
        _clientRepository.Find(saveModel.ReceiverId.Value).Returns(Task.FromResult(receiver));
        _storageRepository.Find(saveModel.StorageId.Value).Returns(Task.FromResult(storage));
        _itemRepository.Create(item).Returns(Task.FromResult(item));

        // Act
        var result = await _itemService.AddAsync(saveModel);

        // Assert
        Assert.Equal(item.Id, result);
        await _clientRepository.Received(1).Find(saveModel.SenderId.Value);
        await _clientRepository.Received(1).Find(saveModel.ReceiverId.Value);
        await _storageRepository.Received(1).Find(saveModel.StorageId.Value);
        await _itemRepository.Received(1).Create(item);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteItem()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // Act
        await _itemService.DeleteAsync(itemId);

        // Assert
        await _itemRepository.Received(1).Delete(itemId);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItems()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = Guid.NewGuid(), Description = "Item 1" },
            new Item { Id = Guid.NewGuid(), Description = "Item 2" }
        };
        var itemReadModels = items.Select(item => new ItemReadModel
        {
            Id = item.Id,
            Description = item.Description,
        });

        _itemRepository.GetAll().Returns(items.AsQueryable());
        _mapper.Map<IEnumerable<ItemReadModel>>(Arg.Any<IEnumerable<Item>>()).Returns(itemReadModels);

        // Act
        var result = await _itemService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(items[0].Id, result.First().Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnItemById()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = new Item { Id = itemId, Description = "Test Item" };
        var itemReadModel = new ItemReadModel { Id = itemId, Description = "Test Item" };

        _itemRepository.Find(itemId).Returns(Task.FromResult(item));
        _mapper.Map<ItemReadModel>(item).Returns(itemReadModel);

        // Act
        var result = await _itemService.GetByIdAsync(itemId);

        // Assert
        Assert.Equal(itemReadModel.Id, result.Id);
    }

    [Fact]
    public async Task GetBySenderPhone_ShouldReturnItemsBySenderPhone()
    {
        // Arrange
        var senderPhone = "123-456-7890";
        var client = new Client { Id = Guid.NewGuid(), Phone = senderPhone };
        var items = new List<Item>
        {
            new Item { Id = Guid.NewGuid(), Description = "Item 1", SenderId = client.Id },
            new Item { Id = Guid.NewGuid(), Description = "Item 2", SenderId = client.Id }
        };
        var itemReadModels = items.Select(item => new ItemReadModel
        {
            Id = item.Id,
            Description = item.Description,
        });

        _clientRepository.GetAll().Returns(new List<Client> { client }.AsQueryable());
        _itemRepository.GetAllAsync(i => i.SenderId == client.Id).Returns(Task.FromResult(items.ToList()));
        _mapper.Map<IEnumerable<ItemReadModel>>(Arg.Any<IEnumerable<Item>>())
            .Returns(itemReadModels);

        // Act
        var result = await _itemService.GetBySenderPhone(senderPhone);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByRecieverPhone_ShouldReturnItemsByReceiverPhone()
    {
        // Arrange
        var receiverPhone = "123-456-7890";
        var client = new Client { Id = Guid.NewGuid(), Phone = receiverPhone };
        var items = new List<Item>
        {
            new Item { Id = Guid.NewGuid(), Description = "Item 1", ReceiverId = client.Id },
            new Item { Id = Guid.NewGuid(), Description = "Item 2", ReceiverId = client.Id }
        };
        var itemReadModels = items.Select(item => new ItemReadModel
        {
            Id = item.Id,
            Description = item.Description,
        });

        _clientRepository.GetAll().Returns(new List<Client> { client }.AsQueryable());
        _itemRepository.GetAllAsync(i => i.SenderId == client.Id).Returns(Task.FromResult(items.ToList()));
        _mapper.Map<IEnumerable<ItemReadModel>>(Arg.Any<IEnumerable<Item>>())
            .Returns(itemReadModels);

        // Act
        var result = await _itemService.GetByRecieverPhone(receiverPhone);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByStorage_ShouldReturnItemsByStorageId()
    {
        // Arrange
        var storageId = Guid.NewGuid();
        var items = new List<Item>
        {
            new Item { Id = Guid.NewGuid(), Description = "Item 1", StorageId = storageId, isReceived = false },
            new Item { Id = Guid.NewGuid(), Description = "Item 2", StorageId = storageId, isReceived = false }
        };
        var itemReadModels = items.Select(item => new ItemReadModel
        {
            Id = item.Id,
            Description = item.Description,
        });

        _itemRepository.GetAllAsync(i => i.StorageId == storageId && !i.isReceived)
            .Returns(Task.FromResult(items));

        _mapper.Map<IEnumerable<ItemReadModel>>(Arg.Any<IEnumerable<Item>>())
            .Returns(itemReadModels);

        // Act
        var result = await _itemService.GetByStorage(storageId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateItemStatus_ShouldUpdateItemStatusAndReturnId()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = new Item { Id = itemId, isReceived = false };

        _itemRepository.Find(itemId).Returns(Task.FromResult(item));
        _itemRepository.Update(item).Returns(Task.FromResult(item));

        // Act
        var result = await _itemService.UpdateItemStatus(itemId, true);

        // Assert
        Assert.Equal(itemId, result);
        Assert.True(item.isReceived);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateItemAndReturnId()
    {
        // Arrange
        var updateModel = new UpdateItemModel
        {
            Id = Guid.NewGuid(),
            Description = "Updated Item",
            SenderId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            StorageId = Guid.NewGuid(),
            CategoryIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };
        var item = new Item { Id = updateModel.Id, Description = "Original Item" };
        var sender = new Client { Id = updateModel.SenderId.Value };
        var receiver = new Client { Id = updateModel.ReceiverId.Value };
        var storage = new Storage { Id = updateModel.StorageId.Value };
        var itemCategories = new List<ItemCategory>{
            new ItemCategory { ItemId = updateModel.Id, CategoryId = updateModel.CategoryIds.First() }
        };
        _itemRepository.Find(updateModel.Id).Returns(Task.FromResult(item));
        _mapper.Map(Arg.Any<UpdateItemModel>(), Arg.Any<Item>()).Returns(item);
        _itemCategoryRepository.GetAllAsync(default).ReturnsForAnyArgs(Task.FromResult(itemCategories));
        _clientRepository.Find(updateModel.SenderId.Value).Returns(Task.FromResult(sender));
        _clientRepository.Find(updateModel.ReceiverId.Value).Returns(Task.FromResult(receiver));
        _storageRepository.Find(updateModel.StorageId.Value).Returns(Task.FromResult(storage));
        _itemRepository.Update(item).Returns(Task.FromResult(item));

        // Act
        var result = await _itemService.UpdateAsync(updateModel);

        // Assert
        Assert.Equal(updateModel.Id, result);
        await _clientRepository.Received(1).Find(updateModel.SenderId.Value);
        await _clientRepository.Received(1).Find(updateModel.ReceiverId.Value);
        await _storageRepository.Received(1).Find(updateModel.StorageId.Value);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}
