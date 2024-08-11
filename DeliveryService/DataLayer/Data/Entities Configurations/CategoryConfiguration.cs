using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Data.Entities_Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(255);

        builder.HasMany(c => c.ItemCategories)
               .WithOne(ic => ic.Category)
               .HasForeignKey(ic => ic.CategoryId);
    }
}
