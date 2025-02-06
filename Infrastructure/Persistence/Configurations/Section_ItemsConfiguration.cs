using Domain.Section_Items;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class Section_ItemsConfiguration : IEntityTypeConfiguration<Section_Items>
{
    public void Configure(EntityTypeBuilder<Section_Items> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.Title).HasColumnType("varchar(255)");
        
        builder.Property(x => x.Content).HasColumnType("varchar(255)");

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        
        builder.HasOne(x => x.Section)
            .WithMany()
            .HasForeignKey(x => x.SectionId)
            .HasConstraintName("fk_section_items_section_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}