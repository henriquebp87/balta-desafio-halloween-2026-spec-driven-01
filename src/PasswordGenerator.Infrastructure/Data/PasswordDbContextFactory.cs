using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PasswordGenerator.Infrastructure.Data;

public sealed class PasswordDbContextFactory : IDesignTimeDbContextFactory<PasswordDbContext>
{
    public PasswordDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<PasswordDbContext>()
            .UseSqlite("Data Source=passwords.db")
            .Options;

        return new PasswordDbContext(options);
    }
}
