using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Data.Entities_Configurations;

public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
{
    public void Configure(EntityTypeBuilder<ItemCategory> builder)
    {
        builder.Property(ic => ic.Id)
            .IsRequired();

        builder.HasKey(ic => new { ic.ItemId, ic.CategoryId });

        builder.HasOne(ic => ic.Item)
               .WithMany(i => i.ItemCategories)
               .HasForeignKey(ic => ic.ItemId);

        builder.HasOne(ic => ic.Category)
               .WithMany(c => c.ItemCategories)
               .HasForeignKey(ic => ic.CategoryId);
    }
}
