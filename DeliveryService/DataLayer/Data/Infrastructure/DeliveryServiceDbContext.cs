using DataLayer.Data.Entities_Configurations;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data.Infrastructure;

public class DeliveryServiceDbContext : DbContext
{
    public DeliveryServiceDbContext(DbContextOptions<DeliveryServiceDbContext> options)
        : base(options)
    {
    }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Storage> Storages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ItemCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new ManagerConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new StorageConfiguration());

        base.OnModelCreating(modelBuilder);

        // Seeding data

        var clientId1 = Guid.NewGuid();
        var clientId2 = Guid.NewGuid();
        var directorId = Guid.NewGuid();
        var storageId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                Id = clientId1,
                Name = "John",
                Phone = "1111"
            },
            new Client
            {
                Id = clientId2,
                Name = "Kyle",
                Phone = "2222"
            }
        );

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = categoryId,
                Description = "Electronics"
            }
        );

        modelBuilder.Entity<Manager>().HasData(
            new Manager
            {
                Id = directorId,
                Name = "Michael",
                StorageId = storageId
            }
        );

        modelBuilder.Entity<Storage>().HasData(
            new Storage
            {
                Id = storageId,
                Address = "456 Maple Ave, Apt 12, Newtown, USA",
                DirectorId = directorId,
                Number = 235
            }
        );

        modelBuilder.Entity<Item>().HasData(
            new Item
            {
                Id = itemId,
                Description = "Laptop ACER Nitro 5",
                Weight = 2.5f,
                Price = 1200m,
                StorageId = storageId,
                SenderId = clientId1,
                ReceiverId = clientId2
            }
        );

        modelBuilder.Entity<ItemCategory>().HasData(
            new ItemCategory
            {
                ItemId = itemId,
                CategoryId = categoryId
            }
        );
    }
}
