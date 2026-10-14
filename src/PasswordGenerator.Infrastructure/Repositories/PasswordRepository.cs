using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Application.Abstractions;
using PasswordGenerator.Infrastructure.Data;
using PasswordEntity = PasswordGenerator.Domain.Entities.Password;

namespace PasswordGenerator.Infrastructure.Repositories;

public sealed class PasswordRepository(PasswordDbContext dbContext) : IPasswordRepository
{
    public async Task AddAsync(
        PasswordEntity password,
        CancellationToken cancellationToken = default)
    {
        dbContext.Passwords.Add(password);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<PasswordEntity?> GetByGuidAsync(
        Guid guid,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Passwords
            .AsNoTracking()
            .SingleOrDefaultAsync(password => password.Guid == guid, cancellationToken);
    }
}
