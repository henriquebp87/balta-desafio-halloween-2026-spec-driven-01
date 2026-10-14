using PasswordGenerator.Application.Abstractions;
using PasswordGenerator.Application.Models;
using PasswordEntity = PasswordGenerator.Domain.Entities.Password;

namespace PasswordGenerator.Application.Services;

public sealed class PasswordService(
    IPasswordGenerator passwordGenerator,
    IPasswordRepository passwordRepository) : IPasswordService
{
    public async Task<PasswordResult> CreateAsync(
        int length,
        CancellationToken cancellationToken = default)
    {
        var generatedPassword = passwordGenerator.Generate(length);
        var password = new PasswordEntity
        {
            Id = Guid.NewGuid(),
            Guid = Guid.NewGuid(),
            Value = generatedPassword,
            CreatedAt = DateTime.UtcNow
        };

        await passwordRepository.AddAsync(password, cancellationToken);

        return new PasswordResult(password.Guid, password.Value);
    }

    public async Task<PasswordResult?> GetByGuidAsync(
        Guid guid,
        CancellationToken cancellationToken = default)
    {
        var password = await passwordRepository.GetByGuidAsync(guid, cancellationToken);

        return password is null
            ? null
            : new PasswordResult(password.Guid, password.Value);
    }
}
