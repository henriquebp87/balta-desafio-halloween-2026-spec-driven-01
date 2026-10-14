using PasswordGenerator.Application.Abstractions;
using PasswordGenerator.Application.Services;
using PasswordEntity = PasswordGenerator.Domain.Entities.Password;

namespace PasswordGenerator.UnitTests;

public sealed class PasswordServiceTests
{
    [Fact]
    public async Task CreateAsync_GeneratesIdentifiersAndPersistsPlainTextPassword()
    {
        const string generatedValue = "Abcdefghijklmn1!";
        var repository = new InMemoryPasswordRepository();
        var service = new PasswordService(new StubPasswordGenerator(generatedValue), repository);
        var startedAt = DateTime.UtcNow;

        var result = await service.CreateAsync(generatedValue.Length);

        var persisted = Assert.Single(repository.Passwords);
        Assert.NotEqual(Guid.Empty, persisted.Id);
        Assert.NotEqual(Guid.Empty, persisted.Guid);
        Assert.Equal(persisted.Guid, result.Guid);
        Assert.Equal(generatedValue, persisted.Value);
        Assert.Equal(generatedValue, result.Password);
        Assert.InRange(persisted.CreatedAt, startedAt, DateTime.UtcNow);
    }

    [Fact]
    public async Task GetByGuidAsync_WhenPasswordExists_ReturnsGuidAndPassword()
    {
        var guid = Guid.NewGuid();
        var repository = new InMemoryPasswordRepository();
        repository.Passwords.Add(new PasswordEntity
        {
            Id = Guid.NewGuid(),
            Guid = guid,
            Value = "Abcdefghijklmn1!",
            CreatedAt = DateTime.UtcNow
        });
        var service = new PasswordService(new StubPasswordGenerator(string.Empty), repository);

        var result = await service.GetByGuidAsync(guid);

        Assert.NotNull(result);
        Assert.Equal(guid, result.Guid);
        Assert.Equal("Abcdefghijklmn1!", result.Password);
    }

    [Fact]
    public async Task GetByGuidAsync_WhenPasswordDoesNotExist_ReturnsNull()
    {
        var service = new PasswordService(
            new StubPasswordGenerator(string.Empty),
            new InMemoryPasswordRepository());

        var result = await service.GetByGuidAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    private sealed class StubPasswordGenerator(string password) : IPasswordGenerator
    {
        public string Generate(int length) => password;
    }

    private sealed class InMemoryPasswordRepository : IPasswordRepository
    {
        public List<PasswordEntity> Passwords { get; } = [];

        public Task AddAsync(
            PasswordEntity password,
            CancellationToken cancellationToken = default)
        {
            Passwords.Add(password);
            return Task.CompletedTask;
        }

        public Task<PasswordEntity?> GetByGuidAsync(
            Guid guid,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Passwords.SingleOrDefault(password => password.Guid == guid));
        }
    }
}
