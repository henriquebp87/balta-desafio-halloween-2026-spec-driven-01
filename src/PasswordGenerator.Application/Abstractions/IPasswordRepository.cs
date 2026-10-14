using PasswordGenerator.Domain.Entities;

namespace PasswordGenerator.Application.Abstractions;

public interface IPasswordRepository
{
    Task AddAsync(Password password, CancellationToken cancellationToken = default);

    Task<Password?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);
}
