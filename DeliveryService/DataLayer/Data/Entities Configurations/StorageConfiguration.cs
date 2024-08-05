using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Data.Entities_Configurations;

public class StorageConfiguration : IEntityTypeConfiguration<Storage>
{
    public void Configure(EntityTypeBuilder<Storage> builder)
    {
        builder.Property(s => s.Id)
            .IsRequired();

        builder.Property(s => s.Address)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Number)
            .IsRequired();

        builder.Property(s => s.Address)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(s => s.Items)
            .WithOne(i => i.Storage)
            .HasForeignKey(i => i.StorageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(s => s.Director)
            .WithOne(d => d.Storage)
            .HasForeignKey<Storage>(s => s.DirectorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Employees)
            .WithOne(e => e.Storage)
            .HasForeignKey(s => s.StorageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
