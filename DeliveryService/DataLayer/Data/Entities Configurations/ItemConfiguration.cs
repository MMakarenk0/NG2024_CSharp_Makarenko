using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Data.Entities_Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(i => i.Id)
                .IsRequired();

            builder.Property(i => i.Price)
                .IsRequired();

            builder.Property(i => i.Description)
                .HasMaxLength(255);

            builder.Property(i => i.Weight)
                .IsRequired();

            builder.HasMany(i => i.ItemCategories)
               .WithOne(ic => ic.Item)
               .HasForeignKey(ic => ic.ItemId);

            builder.HasOne(i => i.Sender)
                .WithMany()
                .HasForeignKey(i => i.SenderId);

            builder.HasOne(i => i.Receiver)
                .WithMany()
                .HasForeignKey(i => i.ReceiverId);

            builder.HasOne(i => i.Storage)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.StorageId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
