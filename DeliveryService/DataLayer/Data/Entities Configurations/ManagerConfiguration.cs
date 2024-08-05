using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data.Entities_Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Manager> builder)
    {
        builder.Property(m => m.Id)
            .IsRequired();

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(m => m.Storage)
            .WithOne(s => s.Director)
            .HasForeignKey<Manager>(m => m.StorageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
