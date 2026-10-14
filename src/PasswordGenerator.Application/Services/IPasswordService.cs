using PasswordGenerator.Application.Models;

namespace PasswordGenerator.Application.Services;

public interface IPasswordService
{
    Task<PasswordResult> CreateAsync(int length, CancellationToken cancellationToken = default);

    Task<PasswordResult?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);
}
