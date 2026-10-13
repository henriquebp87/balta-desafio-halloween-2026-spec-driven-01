using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordGenerator.Domain.Entities;

namespace PasswordGenerator.Infrastructure.Data.Configurations;

public sealed class PasswordConfiguration : IEntityTypeConfiguration<Password>
{
    public void Configure(EntityTypeBuilder<Password> builder)
    {
        builder.ToTable("Passwords");

        builder.HasKey(password => password.Id);

        builder.Property(password => password.Id)
            .ValueGeneratedNever();

        builder.Property(password => password.Guid)
            .IsRequired();

        builder.Property(password => password.Value)
            .HasColumnName("Password")
            .IsRequired();

        builder.Property(password => password.CreatedAt)
            .IsRequired();
    }
}
