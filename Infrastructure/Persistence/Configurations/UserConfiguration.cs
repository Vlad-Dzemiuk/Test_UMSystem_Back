using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.FirstName).HasColumnType("varchar(255)");
        
        builder.Property(x => x.MiddleName).HasColumnType("varchar(255)");

        builder.Property(x => x.LastName).HasColumnType("varchar(255)");

        builder.Property(x => x.Email).IsRequired().HasColumnType("varchar(255)");

        builder.Property(x => x.ProfilePicture).HasColumnType("varchar(255)");

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        
        builder.HasOne(x => x.Gender)
            .WithMany()
            .HasForeignKey(x => x.GenderId)
            .HasConstraintName("fk_users_genders_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}