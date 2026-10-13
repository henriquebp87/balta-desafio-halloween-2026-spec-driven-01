using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Domain.Entities;

namespace PasswordGenerator.Infrastructure.Data;

public sealed class PasswordDbContext(DbContextOptions<PasswordDbContext> options)
    : DbContext(options)
{
    public DbSet<Password> Passwords => Set<Password>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PasswordDbContext).Assembly);
    }
}
