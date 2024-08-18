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
    }
}
