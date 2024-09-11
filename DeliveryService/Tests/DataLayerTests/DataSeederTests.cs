using DataLayer;
using DataLayer.Data.Infrastructure;
using DataLayer.Data.Repositories.Interfaces;
using DataLayer.Entities;
using NSubstitute;

namespace UnitTests.DataLayerTests;

public class DataSeederTests
{
    [Fact]
    public async Task Seed_ShouldCallCreateOnRepositoriesAndSaveChangesAsync()
    {
        // Arrange
        var uof = Substitute.For<IUnitOfWork>();
        var clientRepo = Substitute.For<IClientRepository>();
        var categoryRepo = Substitute.For<ICategoryRepository>();
        var managerRepo = Substitute.For<IManagerRepository>();
        var storageRepo = Substitute.For<IStorageRepository>();
        var itemRepo = Substitute.For<IItemRepository>();
        var itemCategoryRepo = Substitute.For<IItemCategoryRepository>();

        uof.ClientRepository.Returns(clientRepo);
        uof.CategoryRepository.Returns(categoryRepo);
        uof.ManagerRepository.Returns(managerRepo);
        uof.StorageRepository.Returns(storageRepo);
        uof.ItemRepository.Returns(itemRepo);
        uof.ItemCategoryRepository.Returns(itemCategoryRepo);

        var seeder = new DataSeeder(uof);

        // Act
        await seeder.Seed();

        // Assert
        await clientRepo.Received(2).Create(Arg.Any<Client>());

        await categoryRepo.Received(1).Create(Arg.Any<Category>());

        await managerRepo.Received(1).Create(Arg.Any<Manager>());

        await storageRepo.Received(1).Create(Arg.Any<Storage>());

        await itemRepo.Received(1).Create(Arg.Any<Item>());

        await itemCategoryRepo.Received(1).Create(Arg.Any<ItemCategory>());

        await uof.Received(1).SaveChangesAsync();
    }
}
